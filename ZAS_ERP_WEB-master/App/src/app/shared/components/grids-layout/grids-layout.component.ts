import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnDestroy,
  ChangeDetectorRef,
  HostListener,
} from "@angular/core";
import { CommonModule } from "@angular/common";
import { AgGridModule } from "ag-grid-angular";
import { NzModalService, NzModalModule } from "ng-zorro-antd/modal";
import { NzMessageService } from "ng-zorro-antd/message";
import { NzMenuModule } from "ng-zorro-antd/menu";
import {
  DragDropModule,
  CdkDragDrop,
  moveItemInArray,
} from "@angular/cdk/drag-drop";
import { FormsModule } from "@angular/forms";
import { Subscription } from "rxjs";
import { UserSettingService } from "../../services/user-setting.service";

interface GridColumn {
  field: string;
  headerName: string;
  visible: boolean;
  width: number;
}

@Component({
  selector: "app-ag-grid-layout",
  standalone: true,
  imports: [
    CommonModule,
    AgGridModule,
    NzModalModule,
    NzMenuModule,
    DragDropModule,
    FormsModule,
  ],
  templateUrl: "./grids-layout.component.html",
  styleUrls: ["./grids-layout.component.css"],
})
export class AgGridLayoutComponent implements OnInit, OnDestroy {
  // ===== Inputs =====
  @Input() title = "Grid Layout";
  @Input() layoutKey!: string;
  @Input() columnDefs: GridColumn[] = [];
  @Input() rowData: any[] = [];
  @Input() defaultColDef: any = {
    resizable: true,
    sortable: true,
    filter: true,
  };
  @Input() enableSearch = false;
  @Input() searchPlaceholder = "Search...";
  @Input() showActions = true; // show/hide Add/Update/Delete

  // ===== Outputs =====
  @Output() layoutChanged = new EventEmitter<any[]>();
  @Output() rowClicked = new EventEmitter<any>();
  @Output() searchChanged = new EventEmitter<string>();
  @Output() onAdd = new EventEmitter<void>();
  @Output() onUpdate = new EventEmitter<void>();
  @Output() onDelete = new EventEmitter<void>();
  @Output() onRefresh = new EventEmitter<void>();

  // ===== State =====
  colContextMenuVisible = false;
  contextMenuX = 0;
  contextMenuY = 0;
  showColumnsMenu = false;
  private subscriptions = new Subscription();

  selectedRowId: number | null = null;
  gridApi: any;
  gridColumnApi: any;
  searchValue = "";
  isRefreshing = false;
  constructor(
    private userSettingService: UserSettingService,
    private modal: NzModalService,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {}
  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
  totalCount = 0;

  onGridReady(params: any): void {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;

    const events = ["columnMoved", "columnVisible", "columnResized"];
    events.forEach((e) =>
      this.gridApi.addEventListener(e, () => this.captureLayoutState())
    );

    this.loadLayout();
    this.totalCount = this.rowData?.length || 0;

    this.gridApi.addEventListener("modelUpdated", () => {
      this.totalCount = this.gridApi.getDisplayedRowCount();
      this.showCountInPagination();
    });

    setTimeout(() => this.showCountInPagination(), 600);

    setTimeout(() => {
      if (this.gridColumnApi) {
        const allCols = this.gridColumnApi.getAllColumns();
        this.gridColumnApi.autoSizeAllColumns();
      }
    }, 500);

    // ✅ Ensure horizontal scroll
    setTimeout(() => {
      const gridEl = document.querySelector(
        ".ag-grid-container"
      ) as HTMLElement;
      if (gridEl) gridEl.style.overflowX = "auto";
    }, 800);
  }

  triggerRefresh(): void {
    if (this.isRefreshing) return; // prevent double-click spam

    this.isRefreshing = true;
    this.onRefresh.emit(); // emit to parent to actually reload data

    // simulate loader timing (you can stop earlier when data done)
    setTimeout(() => {
      this.isRefreshing = false;
    }, 1500);
  }

  /** ✅ Inject total count label inside AG Grid's pagination bar (Community) */
  private showCountInPagination(): void {
    const paginationPanel = document.querySelector(".ag-paging-panel");
    if (!paginationPanel) return;

    // 🧩 Reuse or create count label
    let countLabel = paginationPanel.querySelector(
      ".custom-count-label"
    ) as HTMLElement;
    if (!countLabel) {
      countLabel = document.createElement("span");
      countLabel.className = "custom-count-label";
      countLabel.style.marginRight = "auto";
      countLabel.style.fontSize = "13px";
      countLabel.style.color = "#495057";
      countLabel.style.fontWeight = "500";
      paginationPanel.prepend(countLabel); // 👈 places on left side
    }

    //  Update label text dynamically
    countLabel.textContent = `💼 Total Count: ${this.totalCount}`;
  }

  /** ================= HEADER CONTEXT MENU HANDLER ================= */
  private attachHeaderContextMenu(): void {
    // Access the header directly from the DOM (Angular-safe)
    const headerRoot = document.querySelector(".ag-header") as HTMLElement;
    if (!headerRoot) {
      // Retry if AG Grid hasn’t rendered header yet
      setTimeout(() => this.attachHeaderContextMenu(), 200);
      return;
    }

    // Prevent duplicate listeners
    headerRoot.removeEventListener("contextmenu", this.handleHeaderContextMenu);
    headerRoot.addEventListener("contextmenu", this.handleHeaderContextMenu);
  }

  /** Handles right-click on header cells */
  private handleHeaderContextMenu = (event: MouseEvent): void => {
    const target = event.target as HTMLElement;
    const isHeaderCell = target.closest(".ag-header-cell");
    if (!isHeaderCell) return;

    event.preventDefault();
    event.stopPropagation();
    this.openColContextMenu(event);
  };

  /** ================= CONTEXT MENU ================= */
  toggleShowColumns(): void {
    this.showColumnsMenu = !this.showColumnsMenu;
  }

  openColContextMenu(event: MouseEvent): void {
    event.preventDefault();
    this.colContextMenuVisible = true;
    this.contextMenuX = event.clientX;
    this.contextMenuY = event.clientY;
  }

  @HostListener("document:click", ["$event"])
  closeContextMenu(event?: MouseEvent): void {
    if (event && event.target instanceof HTMLElement) {
      const inside = event.target.closest(".custom-context-menu");
      if (!inside) this.colContextMenuVisible = false;
    } else {
      this.colContextMenuVisible = false;
    }
  }

  /** ================= TOGGLE COLUMN ================= */
  toggleColumn(col: GridColumn): void {
    col.visible = !col.visible;
    if (this.gridColumnApi) {
      this.gridColumnApi.setColumnVisible(col.field, col.visible);
    }
    this.captureLayoutState();
    this.cdr.detectChanges();
  }

  /** ================= DRAG DROP ================= */
  drop(event: CdkDragDrop<any[]>): void {
    moveItemInArray(this.columnDefs, event.previousIndex, event.currentIndex);
    if (this.gridApi) {
      this.gridApi.setColumnDefs(this.columnDefs);
    }
    this.captureLayoutState();
  }

  /** ================= LOAD LAYOUT ================= */
  loadLayout(): void {
    if (!this.layoutKey) return;

    const localLayout = localStorage.getItem(this.layoutKey);
    if (localLayout) this.applyLayout(localLayout, false);

    this.subscriptions.add(
      this.userSettingService.getSetting(this.layoutKey).subscribe({
        next: (serverLayout) => {
          if (serverLayout) {
            const serverCols = JSON.parse(serverLayout);
            const localCols = localLayout ? JSON.parse(localLayout) : null;
            const isDifferent =
              JSON.stringify(serverCols) !== JSON.stringify(localCols);
            if (isDifferent) {
              this.applyLayout(serverLayout, true);
              localStorage.setItem(this.layoutKey, serverLayout);
            }
          }
        },
        error: () =>
          this.message.warning("⚠️ Could not load layout from server."),
      })
    );
  }

  private applyLayout(layout: string, showMsg = true): void {
    try {
      const savedCols = JSON.parse(layout);

      // Reorder and apply visibility/width
      const reordered = savedCols
        .map((s: any) => {
          const match = this.columnDefs.find((c) => c.field === s.field);
          return match
            ? { ...match, visible: s.visible, width: s.width }
            : null;
        })
        .filter((x: any) => x !== null);

      const newOnes = this.columnDefs.filter(
        (c) => !savedCols.some((s: any) => s.field === c.field)
      );

      this.columnDefs = [...reordered, ...newOnes];

      // Apply to AG Grid
      if (this.gridColumnApi) {
        const colState = this.columnDefs.map((c) => ({
          colId: c.field,
          hide: !c.visible,
          width: c.width,
        }));
        this.gridColumnApi.applyColumnState({
          state: colState,
          applyOrder: true,
        });
        if (this.gridApi) {
          this.gridApi.refreshHeader();
          this.gridApi.refreshCells({ force: true });
        }
      }

      if (showMsg) this.message.success("🔁 Layout restored successfully!");
      this.cdr.detectChanges();
    } catch (err) {
      console.error("Error applying layout:", err);
      this.message.error("❌ Failed to apply saved layout.");
    }
  }

  /** ================= CAPTURE LAYOUT ================= */
  captureLayoutState(): void {
    if (!this.gridColumnApi) return;

    const allCols: GridColumn[] = this.gridColumnApi
      .getAllGridColumns()
      .map((col: any) => ({
        field: col.getColId(),
        visible: col.isVisible(),
        width: col.getActualWidth(),
      }));

    this.columnDefs = this.columnDefs.map((def: GridColumn) => {
      const found = allCols.find((c: GridColumn) => c.field === def.field);
      return found
        ? { ...def, visible: found.visible, width: found.width }
        : def;
    });

    this.layoutChanged.emit(this.columnDefs);
  }

  /** ================= SAVE LAYOUT ================= */
  saveLayout(): void {
    const layout = JSON.stringify(
      this.columnDefs.map(({ field, visible, width }) => ({
        field,
        visible,
        width,
      }))
    );

    this.modal.confirm({
      nzTitle: "Save Grid Layout",
      nzContent: "Do you want to save the current layout?",
      nzOkText: "Save",
      nzOkType: "primary",
      nzCancelText: "Cancel",
      nzOnOk: () => {
        this.subscriptions.add(
          this.userSettingService
            .saveSetting(this.layoutKey, layout)
            .subscribe({
              next: () => {
                localStorage.setItem(this.layoutKey, layout);
                this.message.success("✅ Layout saved successfully!");
              },
              error: () => this.message.error("❌ Failed to save layout."),
            })
        );
      },
    });
  }

  /** ================= SEARCH SUPPORT ================= */

  onSearch(value: string): void {
    this.searchValue = value.trim().toLowerCase();
    if (!this.gridApi) return;

    // v31+ (new API)
    if (typeof (this.gridApi as any).setGridOption === "function") {
      (this.gridApi as any).setGridOption("quickFilterText", this.searchValue);
      return;
    }

    // v30 and below (legacy)
    if (typeof (this.gridApi as any).setQuickFilter === "function") {
      (this.gridApi as any).setQuickFilter(this.searchValue);
      return;
    }

    console.warn("Quick filter API not found on this.gridApi");
  }
  ngOnChanges(): void {
    if (!this.gridApi) return;

    if (typeof (this.gridApi as any).setGridOption === "function") {
      (this.gridApi as any).setGridOption("quickFilterText", this.searchValue);
    } else if (typeof (this.gridApi as any).setQuickFilter === "function") {
      (this.gridApi as any).setQuickFilter(this.searchValue);
    }
  }

  /** ================= ROW CLICK ================= */
  onRowClicked(event: any): void {
    this.selectedRowId = event?.data?.id ?? null; // keep selection for buttons
    this.rowClicked.emit(event);
  }

  /** ================= VISIBLE COLS ================= */
  get visibleColumnDefs(): GridColumn[] {
    return this.columnDefs.filter((col) => col.visible);
  }

  /** ================= ACTION BUTTONS (toolbar) ================= */
  openAddModal(): void {
    this.onAdd.emit();
  }

  openEditModal(): void {
    if (!this.selectedRowId) return;
    this.onUpdate.emit();
  }

  deleteSelected(): void {
    if (!this.selectedRowId) return;
    this.onDelete.emit();
  }

  @HostListener("window:resize")
  onResize(): void {
    if (this.gridApi) this.autoFitColumns();
  }

  private autoFitColumns(): void {
    if (!this.gridColumnApi) return;
    const allColumns = this.gridColumnApi.getAllColumns();
    if (!allColumns?.length) return;

    if (allColumns.length <= 6) {
      // Fewer columns → stretch to fill width
      this.gridApi.sizeColumnsToFit();
    } else {
      // Many columns → auto-size each, allow scroll
      this.gridColumnApi.autoSizeAllColumns();
    }
  }
}
