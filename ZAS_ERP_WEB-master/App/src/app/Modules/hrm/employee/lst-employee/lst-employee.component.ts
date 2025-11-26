import { Component, OnInit, OnDestroy, ChangeDetectorRef } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";
import { NzMessageService } from "ng-zorro-antd/message";
import { Subscription, forkJoin } from "rxjs";
import {
  StatusService,
  TransactionItemType,
  Status,
} from "../../../../shared/services/status.service";
import {
  StatusClassService,
  StatusClass,
} from "../../../../shared/services/statusClass.service";
import {
  EmployeeService,
  Employee,
} from "../../../../shared/services/employee.service";
import { GridComponent } from "../../../../shared/components/grid/grid.component";
import { ColDef, ICellRendererParams } from "ag-grid-community";
import { formatPKTDate } from "../../../../shared/components/dateTime.util";

@Component({
  selector: "app-lst-employee",
  standalone: true,
  imports: [CommonModule, GridComponent],
  templateUrl: "./lst-employee.component.html",
  styleUrls: ["../../../../../scss/lists.css"],
})
export class LstEmployeeComponent implements OnInit, OnDestroy {
  employees: Employee[] = [];
  isLoading = false;
  selectedStatus: string = "all";
  private subscriptions = new Subscription();

  // ===== Grid Columns =====
  columns: ColDef[] = [
    { field: "systemDisplayName", headerName: "Display Name", minWidth: 200 },
    { field: "jobTitle", headerName: "Job Title", minWidth: 180 },
    {
      field: "employeeStatus",
      headerName: "Status",
      minWidth: 180,
      cellRenderer: (params: ICellRendererParams) => {
        const statusName = params.value || "—";
        const color = params.data?.employeeStatusForeColor || "#888";

        return `
          <div style="display:flex;align-items:center;gap:6px;">
            <span
              style="
                width:14px;
                height:14px;
                border-radius:50%;
                background:${color};
                border:1px solid #ccc;
                flex-shrink:0;
              "
            ></span>
            ${statusName}
          </div>
        `;
      },
    },

    // Status Class column: Show ONLY the employee's selected status class
    {
      field: "stagesDisplay",
      headerName: "Status List",
      minWidth: 250,
      cellRenderer: (params: ICellRendererParams) => {
        const list = params.value as StatusClass[] | undefined;
        if (!list || list.length === 0)
          return `<span style="color:#aaa;">—</span>`;

        const cls = list[0]; // Only one class
        const color = cls.foreColor || "#888";

        return `
      <div style="display:flex;align-items:center;gap:6px;">
        <span
          style="
            width:14px;
            height:14px;
            border-radius:50%;
            background:${color};
            border:1px solid #ccc;
            flex-shrink:0;
          "
        ></span>
        ${cls.className}
      </div>`;
      },
    },

    // Open/Closed column: Show status of the selected class only
    {
      field: "openClosedDisplay",
      headerName: "Open/Closed",
      minWidth: 150,
      cellRenderer: (params: ICellRendererParams) => {
        const list = params.value as StatusClass[] | undefined;
        if (!list || list.length === 0)
          return `<span style="color:#aaa;">—</span>`;

        const cls = list[0];
        return `
          <span class="badge ${cls.isActive ? "bg-success" : "bg-secondary"}">
            ${cls.isActive ? "Open" : "Closed"}
          </span>`;
      },
    },

    { field: "employmentType", headerName: "Employment Type", minWidth: 180 },
    { field: "hrManager", headerName: "HR Manager", minWidth: 150 },
    { field: "manager", headerName: "Manager", minWidth: 150 },
    { field: "hireDate", headerName: "Hire Date", minWidth: 180 },
    { field: "joinDate", headerName: "Join Date", minWidth: 180 },
    {
      field: "createdDate",
      headerName: "Created On",
      minWidth: 200,
      valueFormatter: (p) => formatPKTDate(p.value),
    },
    {
      field: "createdByUserName",
      headerName: "Created By",
      minWidth: 150,
      cellRenderer: (p: ICellRendererParams) => p.value || "—",
    },
    {
      field: "lastModifiedDate",
      headerName: "Modified On",
      minWidth: 200,
      valueFormatter: (p) => formatPKTDate(p.value),
    },
    {
      field: "lastModifiedByUserName",
      headerName: "Modified By",
      minWidth: 150,
      cellRenderer: (p: ICellRendererParams) => p.value || "—",
    },
    {
      field: "isActive",
      headerName: "Is Active",
      minWidth: 100,
      cellRenderer: (params: ICellRendererParams) => {
        return params.value
          ? `<span class="badge bg-success">Active</span>`
          : `<span class="badge bg-danger">Inactive</span>`;
      },
    },
    { field: "contact.email", headerName: "Email", minWidth: 220 },
    { field: "contact.phoneNumber", headerName: "Phone", minWidth: 150 },
    {
      field: "permanentAddress.addressLine1",
      headerName: "Address",
      minWidth: 250,
    },
  ];

  gridLayoutKey = "EmployeeGridLayout";

  constructor(
    private employeeService: EmployeeService,
    private router: Router,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef,
    private statusService: StatusService,
    private statusClassService: StatusClassService
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  // ===== Load Employee Data =====
  loadEmployees(): void {
    this.isLoading = true;

    const employees$ = this.employeeService.getAllEmployees(
      this.selectedStatus
    );
    const statuses$ = this.statusService.getAllStatuses(
      TransactionItemType.Employee,
      "all"
    );
    const statusClasses$ = this.statusClassService.getAll(
      TransactionItemType.Employee,
      "all"
    );

    this.subscriptions.add(
      forkJoin([employees$, statuses$, statusClasses$]).subscribe({
        next: ([employees, statuses, statusClasses]) => {
          // Map: statusName → foreColor (for Status column)
          const statusColorMap = new Map(
            statuses.map((s) => [s.statusName, s.foreColor])
          );

          // Map: className → StatusClass object (for lookup)
          const classMap = new Map<string, StatusClass>();
          statusClasses.forEach((cls) => {
            if (cls.className) {
              classMap.set(cls.className, cls);
            }
          });

          // Process each employee
          this.employees = (employees || []).map((emp) => {
            const empStatus = emp.employeeStatus ?? "";
            const selectedClassName = emp.employeeStatusClass ?? "";

            // Find the exact StatusClass that matches employee's selected class
            const selectedClass = classMap.get(selectedClassName);

            return {
              ...emp,
              manager: emp.managerName ?? "—",
              hrManager: emp.hrManagerName ?? "—",
              employeeStatusForeColor: statusColorMap.get(empStatus),

              // Only show the employee's selected status class
              stagesDisplay: selectedClass ? [selectedClass] : [],
              openClosedDisplay: selectedClass ? [selectedClass] : [],
            };
          });

          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error("Error loading employees:", err);
          this.message.error("Failed to load employees.");
          this.isLoading = false;
        },
      })
    );
  }

  // ===== Filter by Status =====
  setFilterState(status: string): void {
    this.selectedStatus = status;
    this.loadEmployees();
  }

  // ===== Navigation =====
  navigateToCreateEmployee(): void {
    this.router.navigate(["/employee-form"]);
  }

  navigateToUpdateEmployee(selected: Employee): void {
    if (!selected || !selected.id) {
      this.message.warning("Please select exactly ONE employee to update.");
      return;
    }
    this.router.navigate(["/employee-form", { id: selected.id }]);
  }

  refreshEmployees(): void {
    this.loadEmployees();
    this.message.success("Employee list refreshed successfully!");
  }
}
