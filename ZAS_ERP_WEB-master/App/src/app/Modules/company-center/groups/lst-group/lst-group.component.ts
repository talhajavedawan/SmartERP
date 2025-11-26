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

import { GroupService } from "../../../../shared/services/company-center/groups/group.service";
import { Group } from "../group.model";
import { UserSettingService } from "../../../../shared/services/user-setting.service";
import { GridComponent } from "../../../../shared/components/grid/grid.component";
import { ColDef, ICellRendererParams } from "ag-grid-community";

@Component({
  selector: "app-lst-group",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzModalModule, GridComponent],
  templateUrl: "./lst-group.component.html",
  styleUrls: ["../../../../../scss/lists.css"],
})
export class LstGroupComponent implements OnInit, OnDestroy {
  // ────── STATE ──────
  groups: Group[] = [];
  filteredGroups: Group[] = [];
  isLoading = false;
  selectedStatus: string = "all";

  readonly GRID_LAYOUT_KEY = "group-grid-layout";

  private subscriptions = new Subscription();

  // ────── GRID CONFIG ──────
  columns: ColDef[] = [
    {
      field: "groupName",
      headerName: "Group Name",
      minWidth: 200,
      filter: "agTextColumnFilter",
    },
    {
      field: "isActive",
      headerName: "Status",
      minWidth: 110,
      cellRenderer: (params: ICellRendererParams) => {
        const active = params.value === true;
        return `<span class="badge ${active ? "bg-success" : "bg-danger"}">
                  ${active ? "Active" : "Inactive"}
                </span>`;
      },
    },
    {
      field: "createdByName",
      headerName: "Created By",
      minWidth: 150,
      filter: "agTextColumnFilter",
    },
    {
      field: "creationDate",
      headerName: "Created On",
      minWidth: 180,
      filter: "agDateColumnFilter",
    },
    {
      field: "modifiedByName",
      headerName: "Modified By",
      minWidth: 150,
      filter: "agTextColumnFilter",
    },
    {
      field: "lastModified",
      headerName: "Last Modified",
      minWidth: 180,
      filter: "agDateColumnFilter",
    },
  ];

  // ────── INJECTIONS ──────
  constructor(
    private groupService: GroupService,
    private message: NzMessageService,
    private userSettingService: UserSettingService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    @Inject(DOCUMENT) private document: Document
  ) {}

  // ────── LIFECYCLE ──────
  ngOnInit(): void {
    this.loadGroups();
    // No extra debounce needed – <app-grid> already debounces its own search
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  // ────── DATA LOADING ──────
  loadGroups(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.groupService.getAllGroups(this.selectedStatus).subscribe({
        next: (data: Group[]) => {
          this.groups = data || [];
          this.filteredGroups = [...this.groups];
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error(err);
          this.isLoading = false;
          this.message.error("Failed to load groups.");
          this.cdr.detectChanges();
        },
      })
    );
  }

  // ────── STATUS FILTER ──────
  setStatusFilter(status: string): void {
    this.selectedStatus = status;
    this.loadGroups();
  }

  // ────── ROW SELECTION ──────
  onRowSelected(group: Group | null): void {
    if (group) {
      this.selectGroup(group);
    } else {
      this.document.defaultView?.sessionStorage.removeItem("selectedGroup");
    }
    this.cdr.detectChanges();
  }

  private selectGroup(group: Group): void {
    this.document.defaultView?.sessionStorage.setItem(
      "selectedGroup",
      JSON.stringify(group)
    );
  }

  private getSelectedGroup(): Group | null {
    const stored =
      this.document.defaultView?.sessionStorage.getItem("selectedGroup");
    return stored ? JSON.parse(stored) : null;
  }

  // ────── CRUD NAVIGATION ──────
  navigateToCreate(): void {
    this.document.defaultView?.sessionStorage.removeItem("selectedGroup");
    this.router.navigate(["/frmGroups"]);
  }

  navigateToEdit(group?: Group | null): void {
    const target = group ?? this.getSelectedGroup();
    if (!target?.id) {
      this.message.warning("Please select a group to edit.");
      return;
    }
    this.document.defaultView?.sessionStorage.setItem(
      "selectedGroup",
      JSON.stringify(target)
    );
    this.router.navigate(["/frmGroups"], { queryParams: { id: target.id } });
  }

  // ────── GRID EVENTS ──────
  onSearch(term: string): void {
    const lower = term.trim().toLowerCase();
    this.filteredGroups = this.groups.filter((g) =>
      (g.groupName || "").toLowerCase().includes(lower)
    );
    this.cdr.detectChanges();
  }

  refreshGroups(): void {
    this.loadGroups();
    this.message.success("Group list refreshed!");
  }

  // ────── LAYOUT SAVE (called from grid) ──────
  onLayoutChanged(): void {
    // The grid already saves via its own `saveLayout()` – we just keep the key in sync
    this.message.success("Layout saved!");
  }
}
