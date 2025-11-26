import { Component, OnInit, OnDestroy, ChangeDetectorRef } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";
import { NzMessageService } from "ng-zorro-antd/message";
import { Subscription } from "rxjs";

import {
  CustomerService,
  CustomerDTO,
} from "../../../shared/services/customer.service";
import { GridComponent } from "../../../shared/components/grid/grid.component";
import { ColDef, ICellRendererParams } from "ag-grid-community";
@Component({
  selector: "app-lst-customer",
  standalone: true,
  imports: [CommonModule, GridComponent],
  templateUrl: "./lst-customer.component.html",
  styleUrls: ["../../../../scss/lists.css"],
})
export class LstCustomerComponent implements OnInit, OnDestroy {
  customers: CustomerDTO[] = [];
  isLoading = false;
  selectedStatus: string = "all";
  private subscriptions = new Subscription();

  // ===== Grid Columns =====
  columns: ColDef[] = [
    { field: "companyName", headerName: "Company Name", minWidth: 200 },
    { field: "businessTypeName", headerName: "Business Type", minWidth: 180 },
    { field: "industryTypeName", headerName: "Industry Type", minWidth: 180 },
    { field: "contactPersonName", headerName: "Contact Person", minWidth: 150 },
    { field: "contactEmail", headerName: "Email", minWidth: 220 },
    { field: "contactPhone", headerName: "Phone", minWidth: 150 },
    { field: "billingAddress", headerName: "Billing Address", minWidth: 250 },
    { field: "shippingAddress", headerName: "Shipping Address", minWidth: 250 },
    { field: "createdDate", headerName: "Created Date", minWidth: 180 },
    {
      field: "isActive",
      headerName: "Status",
      minWidth: 100,

      cellRenderer: (params: ICellRendererParams) => {
        const isActive = params.value;
        if (isActive) {
          return `<span class="badge bg-success">Active</span>`;
        } else {
          return `<span class="badge bg-danger">Inactive</span>`;
        }
      },
    },
  ];

  // ===== Layout Key =====
  gridLayoutKey = "CustomerGridLayout"; // Unique key for this grid

  constructor(
    private customerService: CustomerService,
    private router: Router,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadCustomers();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  // ===== Load Customer Data =====
  loadCustomers(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.customerService.getAllCustomers(this.selectedStatus).subscribe({
        next: (data: CustomerDTO[]) => {
          this.customers = data || [];
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error("Error loading customers:", err);
          this.message.error("Failed to load customers.");
          this.isLoading = false;
        },
      })
    );
  }
  // ===== Filter by Status =====
  setStatusFilter(status: string): void {
    this.selectedStatus = status;
    this.loadCustomers();
  }
  // ===== Navigation =====
  navigateToCreateCustomer(): void {
    this.router.navigate(["/frm-customer"]);
  }

  navigateToUpdateCustomer(selected: CustomerDTO): void {
    if (!selected || !selected.id) {
      this.message.warning("⚠️ Please select exactly ONE customer to update.");
      return;
    }
    this.router.navigate(["/frm-customer", { id: selected.id }]);
  }
  refreshCustomers(): void {
    this.loadCustomers();
    this.message.success("Customer list refreshed successfully!");
  }
}
