import { Component, OnInit, ChangeDetectorRef } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClientModule, HttpErrorResponse } from "@angular/common/http";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzMessageService } from "ng-zorro-antd/message";
import { Router, RouterModule } from "@angular/router";
import { LocationService } from "../../../shared/services/location.service";
import { AgGridLayoutComponent } from "../../../shared/components/grids-layout/grids-layout.component";
import { NzPaginationModule } from "ng-zorro-antd/pagination";
import { GridLayoutComponent } from "../../../shared/components/grid-layout/grid-layout.component";

/**
 * ✅ Zone List Component
 * Displays all zones in a grid and provides Add/Edit/Delete actions.
 */
@Component({
  selector: "app-zone",
  standalone: true,
  templateUrl: "./zone.component.html",
  styleUrls: ["./zone.component.css"],
  imports: [
    CommonModule,
    HttpClientModule,
    NzButtonModule,
    RouterModule,
    GridLayoutComponent,
    NzPaginationModule
  ],
})
export class ZoneComponent implements OnInit {
  zones: any[] = [];
  selectedZone: any = null;
  isLoading = false;
detailPanelWidth: string = "185px";

  /** ✅ AG Grid Column Definitions */
  columnDefs = [
    {
      field: "name",
      headerName: "Zone Name",
      width: 400,
      visible: true,
    },
    {
      field: "countries",
      headerName: "Countries Count",
      width: 250,
      visible: true,
      valueGetter: (params: any) => params.data?.countries?.length || 0,
    },
  
  ];

  constructor(
    private locationService: LocationService,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  /** ===================== INIT ===================== */
  ngOnInit(): void {
    this.loadZones();
  }

  /** ===================== LOAD ALL ZONES ===================== */
  loadZones(): void {
    this.isLoading = true;
    this.locationService.getAllZones().subscribe({
      next: (res: any) => {
        // ✅ Handle both { success, data } and plain array
        this.zones = Array.isArray(res) ? res : res?.data ?? [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: HttpErrorResponse) => {
        console.error("Error loading zones:", err);
        this.message.error("❌ Failed to load zones.");
        this.isLoading = false;
      },
    });
  }

  /** ===================== SELECT ROW ===================== */
  onRowClicked(event: any): void {
    this.selectedZone = event?.data;
  }

  /** ===================== DOUBLE-CLICK ROW TO EDIT ===================== */
  onRowDoubleClicked(event: any): void {
    const zone = event?.data;
    if (zone) {
      this.router.navigate(["/frmZone"], {
        queryParams: { id: zone.id },
      });
    }
  }

  /** ===================== ADD NEW ZONE ===================== */
  onAdd(): void {
    this.router.navigate(["/frmZone"]);
  }

  /** ===================== EDIT SELECTED ZONE ===================== */
  onUpdate(): void {
    if (!this.selectedZone) {
      this.message.warning("Please select a zone to edit.");
      return;
    }

    this.router.navigate(["/frmZone"], {
      queryParams: { id: this.selectedZone.id },
    });
  }

  /** ===================== DELETE ZONE ===================== */
  onDelete(): void {
    if (!this.selectedZone) {
      this.message.warning("Please select a zone to delete.");
      return;
    }

    this.isLoading = true;
    this.locationService.deleteZone(this.selectedZone.id).subscribe({
      next: () => {
        this.message.success("✅ Zone deleted successfully!");
        this.selectedZone = null;
        this.loadZones();
      },
      error: (err) => {
        this.isLoading = false;
        console.error("Delete zone error:", err);
        this.message.error("❌ Failed to delete zone.");
      },
    });
  }

  /** ===================== REFRESH ZONES ===================== */
  refreshZones(): void {
    this.loadZones();
    this.message.success("✅ Zone list refreshed successfully!");
  }

  onSearch(event: any) {
  const value = event.target.value.toLowerCase();
  this.zones = this.zones.map(z => ({ ...z })); // force refresh

  if (!value) {
    this.loadZones();
    return;
  }

  this.zones = this.zones.filter(z =>
    z.name?.toLowerCase().includes(value)
  );
}


isResizing = false;

startResizing(event: MouseEvent) {
  this.isResizing = true;

  const startX = event.clientX;
  const startWidth = parseInt(this.detailPanelWidth, 10);

  const mouseMoveHandler = (moveEvent: MouseEvent) => {
    if (!this.isResizing) return;

    const newWidth = startWidth - (moveEvent.clientX - startX);

    // 🔥 Set min/max width limits
    if (newWidth > 220 && newWidth < 600) {
      this.detailPanelWidth = newWidth + "px";
    }
  };

  const mouseUpHandler = () => {
    this.isResizing = false;
    document.removeEventListener("mousemove", mouseMoveHandler);
    document.removeEventListener("mouseup", mouseUpHandler);
  };

  document.addEventListener("mousemove", mouseMoveHandler);
  document.addEventListener("mouseup", mouseUpHandler);
}


}
