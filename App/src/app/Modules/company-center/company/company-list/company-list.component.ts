import { Component, OnInit, OnDestroy, ChangeDetectorRef } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";
import { NzModalModule } from "ng-zorro-antd/modal";
import { NzMessageService } from "ng-zorro-antd/message";
import { Subscription } from "rxjs";
import { ColDef } from "ag-grid-community";

import { CompanyService } from "../../../../shared/services/company-center/company/company.service";
import { GridComponent } from "../../../../shared/components/grid/grid.component";

@Component({
  selector: "app-lst-company",
  standalone: true,
  imports: [CommonModule, NzModalModule, GridComponent],
  templateUrl: "./company-list.component.html",
  styleUrls: ["../../../../../scss/lists.css"],
})
export class LstCompanyComponent implements OnInit, OnDestroy {
  selectedStatus = "all";
  rowData: any[] = [];
  isLoading = false;

  readonly GRID_LAYOUT_KEY = "company-grid-layout";
  private subscriptions = new Subscription();

  /** Full tree structure (never modified after load) */
  private fullTree: any[] = [];

  columnDefs: ColDef[] = [
    {
      field: "companyName",
      headerName: "Company Name",
      width: 340,
      cellRenderer: this.getCompanyNameCellRenderer.bind(this),
      filter: "agTextColumnFilter",
    },
    {
      field: "ntn",
      headerName: "NTN",
      width: 140,
      filter: "agTextColumnFilter",
    },
    {
      field: "industryTypeName",
      headerName: "Industry",
      width: 160,
      filter: "agTextColumnFilter",
    },
    {
      field: "businessTypeName",
      headerName: "Business Type",
      width: 160,
      filter: "agTextColumnFilter",
    },
    {
      field: "groupName",
      headerName: "Group",
      width: 140,
      filter: "agTextColumnFilter",
    },
    {
      field: "companyType",
      headerName: "Type",
      width: 140,
      valueFormatter: (params) => {
        const type = params.value;
        switch (type) {
          case 1:
            return "Group";
          case 2:
            return "Individual";
          case 3:
            return "Customer";
          case 4:
            return "Vendor";
          case 5:
            return "Principal";
          default:
            return "—";
        }
      },
      cellRenderer: (params: any) => {
        const type = params.value;
        let badgeClass = "badge bg-secondary";
        let text = "—";

        switch (type) {
          case 1:
            badgeClass = "badge bg-primary";
            text = "Group";
            break;
          case 2:
            badgeClass = "badge bg-info";
            text = "Individual";
            break;
          case 3:
            badgeClass = "badge bg-success";
            text = "Customer";
            break;
          case 4:
            badgeClass = "badge bg-warning text-dark";
            text = "Vendor";
            break;
          case 5:
            badgeClass = "badge bg-danger";
            text = "Principal";
            break;
        }

        return `<span class="${badgeClass}">${text}</span>`;
      },
      filter: "agTextColumnFilter",
    },
    {
      field: "isActive",
      headerName: "Status",
      width: 110,
      cellRenderer: (p: { value: boolean }) =>
        p.value
          ? '<span class="badge bg-success">Active</span>'
          : '<span class="badge bg-danger">Inactive</span>',
    },
    {
      field: "isSubsidiary",
      headerName: "Subsidiary",
      width: 110,
      cellRenderer: (p: { value: boolean }) =>
        p.value
          ? '<span class="badge bg-info">Yes</span>'
          : '<span class="badge bg-secondary">No</span>',
    },
    {
      field: "parentCompanyName",
      headerName: "Parent",
      width: 180,
      filter: "agTextColumnFilter",
    },
    {
      field: "zoneName",
      headerName: "Zone",
      width: 140,
      filter: "agTextColumnFilter",
    },
    {
      field: "countryName",
      headerName: "Country",
      width: 130,
      filter: "agTextColumnFilter",
    },
    {
      field: "stateName",
      headerName: "State",
      width: 130,
      filter: "agTextColumnFilter",
    },
    {
      field: "cityName",
      headerName: "City",
      width: 130,
      filter: "agTextColumnFilter",
    },
    {
      field: "zipCode",
      headerName: "Zip",
      width: 100,
      filter: "agTextColumnFilter",
    },
    {
      field: "addressLine1",
      headerName: "Address 1",
      width: 200,
      filter: "agTextColumnFilter",
    },
    {
      field: "addressLine2",
      headerName: "Address 2",
      width: 200,
      filter: "agTextColumnFilter",
    },
    {
      field: "phoneNumber",
      headerName: "Phone",
      width: 150,
      filter: "agTextColumnFilter",
    },
    {
      field: "email",
      headerName: "Email",
      width: 200,
      filter: "agTextColumnFilter",
    },
    {
      field: "websiteUrl",
      headerName: "Website",
      width: 200,
      filter: "agTextColumnFilter",
    },
  ];

  constructor(
    private companyService: CompanyService,
    private router: Router,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadCompanies();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  // ── Load & Build Tree (ONCE) ──
  loadCompanies(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.companyService.getAllCompanies(this.selectedStatus).subscribe({
        next: (data: any[]) => {
          const arr = Array.isArray(data) ? data : [];
          const map = new Map<number, any>();
          const roots: any[] = [];

          // 1. Clone + add UI fields + flatten nested objects
          arr.forEach((c: any) => {
            const address = c.address || {};
            const contact = c.contact || {};
            const industry = c.industryType || {};
            const business = c.businessType || {};
            const group = c.group || {};
            const parent = c.parentCompany || {};

            const node = {
              id: c.id,
              parentCompanyId: c.parentCompanyId || null,
              companyName: c.companyName || "—",
              ntn: c.ntn || "—",
              industryTypeName: industry.industryTypeName || "—",
              businessTypeName: business.businessTypeName || "—",
              groupName: group.groupName || "—",
              companyType: c.companyType,
              isActive: c.isActive,
              isSubsidiary: c.isSubsidiary,
              parentCompanyName: parent.companyName || "—",

              // Address
              zoneName: address.zone?.name || address.zoneName || "—",
              countryName: address.country?.name || address.countryName || "—",
              stateName: address.state?.name || address.stateName || "—",
              cityName: address.city?.name || address.cityName || "—",
              zipCode: address.zipcode || "—",
              addressLine1: address.addressLine1 || "—",
              addressLine2: address.addressLine2 || "—",

              // Contact
              phoneNumber: contact.phoneNumber || "—",
              email: contact.email || "—",
              websiteUrl: contact.websiteUrl || "—",

              // Tree UI
              level: 0,
              expand: false,
              children: [],
            };
            map.set(c.id, node);
            if (!c.parentCompanyId) {
              roots.push(node);
            }
          });

          // 2. Attach children + set level
          arr.forEach((c: any) => {
            if (c.parentCompanyId) {
              const parent = map.get(c.parentCompanyId);
              const child = map.get(c.id);
              if (parent && child) {
                child.level = parent.level + 1;
                parent.children.push(child);
              }
            }
          });

          // 3. Save full tree
          this.fullTree = roots;

          // 4. Flatten initially (collapsed)
          this.rowData = this.flattenTree(this.fullTree);
          this.isLoading = false;
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error(err);
          this.isLoading = false;
          this.message.error("Failed to load companies.");
        },
      })
    );
  }

  /** Flatten tree based on expand state */
  private flattenTree(nodes: any[], result: any[] = []): any[] {
    nodes.forEach((node) => {
      result.push(node);
      if (node.expand && node.children.length > 0) {
        this.flattenTree(node.children, result);
      }
    });
    return result;
  }

  getCompanyNameCellRenderer(params: any): string {
    const company = params.data;
    const indent = company.level * 22;
    const hasChildren = company.children && company.children.length > 0;
    const icon = hasChildren
      ? `<i class="bi ${
          company.expand ? "bi-caret-down-fill" : "bi-caret-right-fill"
        } text-primary toggle-icon" data-id="${company.id}"></i>`
      : "";

    return `
    <span style="display:inline-flex; align-items:center; gap:6px; padding-left:${indent}px;">
      ${icon}
      <span class="company-name" data-id="${company.id}">${company.companyName}</span>
    </span>
  `;
  }

  onRowClicked(event: any): void {
    const company = event.data;

    const clickedElement = event.event?.target as HTMLElement;
    if (clickedElement && clickedElement.classList.contains("toggle-icon")) {
      // Expand/collapse only when clicking icon
      company.expand = !company.expand;
      this.rowData = this.flattenTree(this.fullTree);
      this.cdr.markForCheck();
      return;
    }

    // Normal row click = select
    this.gridSelectRow(event);
  }

  private gridSelectRow(event: any): void {
    if (event.node) {
      event.node.setSelected(true, true);
    }
    const company = event.data;
    sessionStorage.setItem("selectedCompany", JSON.stringify(company));
  }

  // ── Navigation ──
  navigateToCreateCompany(): void {
    sessionStorage.removeItem("selectedCompany");
    this.router.navigate(["/company-create-update"]);
  }

  navigateToUpdateCompany(company: any): void {
    if (!company?.id) {
      this.message.warning("Please select a company.");
      return;
    }
    this.router.navigate([`/company-create-update/${company.id}`], {
      state: { company },
    });
  }

  refreshCompanies(): void {
    this.loadCompanies();
    this.message.success("Companies refreshed!");
  }

  setFilterState(status: string): void {
    this.selectedStatus =
      status === "active"
        ? "active"
        : status === "inactive"
        ? "inactive"
        : "all";
    this.loadCompanies();
  }
}