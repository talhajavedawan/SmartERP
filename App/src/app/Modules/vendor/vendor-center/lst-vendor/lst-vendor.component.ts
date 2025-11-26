import {
  Component,
  OnInit,
  OnDestroy,
  ChangeDetectorRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { NzMessageService } from 'ng-zorro-antd/message';
import { VendorService } from '../../../../shared/services/vendor/vendor.service';
import { VendorGetDto } from '../vendor.model';
import { ColDef, ICellRendererParams } from 'ag-grid-community';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { GridComponent } from '../../../../shared/components/grid/grid.component';

interface VendorNode extends VendorGetDto {
  expand?: boolean;
  level?: number;
  children?: VendorNode[];
}

@Component({
  selector: 'app-lst-vendor',
  standalone: true,
  imports: [CommonModule, GridComponent,NzPaginationModule],
  templateUrl: './lst-vendor.component.html',
  styleUrls: ['./lst-vendor.component.css'],
})
export class LstVendorComponent implements OnInit, OnDestroy {
  vendors: VendorNode[] = [];
  displayVendors: VendorNode[] = [];
  isLoading = false;
  selectedStatus: string = 'all';
  private subs = new Subscription();
selectedVendor: VendorNode | null = null;
 pageNumber = 1;
 pageSize = 10;
 sortColumn = 'companyName';
 sortDirection = 'asc';
 searchTerm = '';
 totalCount = 0;
 totalPages = 0;

  gridLayoutKey = 'VendorGridLayout';

columnDefs: ColDef[] = [
  {
    headerName: 'Vendor Name',
    field: 'companyName',
    minWidth: 250,
    cellRenderer: this.getVendorNameCellRenderer.bind(this),
    onCellClicked: this.onVendorCellClick.bind(this),
  },
  { field: 'ntn', headerName: 'NTN', minWidth: 140 },
  { field: 'registrationNumber', headerName: 'Reg. Number', minWidth: 160 },
  { field: 'businessTypeName', headerName: 'Business Type', minWidth: 180 },
  { field: 'industryTypeName', headerName: 'Industry Type', minWidth: 180 },
  { field: 'vendorNature', headerName: 'Nature', minWidth: 130 },
  { field: 'currency', headerName: 'Currency', minWidth: 100 },

  { field: 'contactEmail', headerName: 'Email', minWidth: 220 },
  { field: 'contactPhone', headerName: 'Phone', minWidth: 150 },
  { field: 'contactWebsiteUrl', headerName: 'Website', minWidth: 200 },

  // ✅ Correct field names (not AddressLine1)
  { field: 'billingAddress', headerName: 'Billing Address', minWidth: 250 },
  { field: 'billingZipcode', headerName: 'Billing Zip', minWidth: 120 },
  { field: 'shippingAddress', headerName: 'Shipping Address', minWidth: 250 },
  { field: 'shippingZipcode', headerName: 'Shipping Zip', minWidth: 120 },

  { 
    field: 'ranking',
    headerName: 'Ranking',
    minWidth: 100,
    cellRenderer: (p: ICellRendererParams) => {
      const r = Number(p.value);
      if (isNaN(r) || r <= 0) return `<span class="badge bg-secondary">—</span>`;
      const cls = ['bg-danger', 'bg-warning text-dark', 'bg-info text-dark', 'bg-success', 'bg-primary'][r - 1] ?? 'bg-secondary';
      return `<span class="badge ${cls}">${r}</span>`;
    },
  },

  {
    field: 'isSubsidiary',
    headerName: 'Subsidiary',
    minWidth: 110,
    cellRenderer: (p: ICellRendererParams) => {
      const yes = p.value === true;
      return `<span class="badge ${yes ? 'bg-success-subtle text-success' : 'bg-secondary-subtle text-muted'}">
                ${yes ? 'Yes' : 'No'}
              </span>`;
    },
  },
  {
    field: 'redList',
    headerName: 'Red List',
    minWidth: 100,
    cellRenderer: (p: ICellRendererParams) => {
      const yes = p.value === true;
      return `<span class="badge ${yes ? 'bg-danger' : 'bg-secondary'}">${yes ? 'Yes' : 'No'}</span>`;
    },
  },
  {
    field: 'isActive',
    headerName: 'Status',
    minWidth: 110,
    cellRenderer: (p: ICellRendererParams) => {
      const active = p.value === true;
      return `<span class="badge ${active ? 'bg-success' : 'bg-danger'}">
                ${active ? 'Active' : 'Inactive'}
              </span>`;
    },
  },

  { field: 'createdDate', headerName: 'Created Date', minWidth: 180 },

  // ✅ These are correctly named in your backend
  { field: 'parentVendorName', headerName: 'Parent Vendor', minWidth: 180 },

  // Arrays need joining for display
  {
    field: 'clientCompanyNames',
    headerName: 'Client Companies',
    minWidth: 220,
    valueFormatter: (p) =>
      Array.isArray(p.value) && p.value.length > 0 ? p.value.join(', ') : '—',
  },
  {
    field: 'departmentNames',
    headerName: 'Departments',
    minWidth: 220,
    valueFormatter: (p) =>
      Array.isArray(p.value) && p.value.length > 0 ? p.value.join(', ') : '—',
  },
];


  constructor(
    private vendorService: VendorService,
    private router: Router,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadVendors();
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  setStatusFilter(status: string): void {
    this.selectedStatus = status;
    this.loadVendors();
  }

  private loadVendors(): void {
  this.isLoading = true;
  this.vendors = [];
  this.displayVendors = [];

  this.subs.add(
    this.vendorService
      .getAllVendors(
        this.selectedStatus,
        this.pageNumber,
        this.pageSize,
        this.sortColumn,
        this.sortDirection,
        this.searchTerm
      )
      .subscribe({
        next: (res) => {
          const response = res?.data ?? [];
          const list: VendorGetDto[] = Array.isArray(response)
            ? response
            : response.$values ?? [];

          const flat: VendorNode[] = list.map(v => ({
            ...v,
            parentVendorId: v.parentVendorId ?? undefined,
            expand: false,
            level: 0,
            children: [],
          }));

          this.vendors = this.buildTree(flat);
          this.displayVendors = this.flattenTree(this.vendors);
          this.totalCount = res.totalCount ?? 0;
          this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.isLoading = false;
          this.message.error('Failed to load vendors');
        },
      })
  );
}


  private buildTree(nodes: VendorNode[]): VendorNode[] {
    const map = new Map<number, VendorNode>();
    const roots: VendorNode[] = [];

    nodes.forEach(n => {
      map.set(n.id, n);
      n.children = [];
      n.level = 0;
    });

    nodes.forEach(n => {
      if (n.parentVendorId != null) {
        const parent = map.get(n.parentVendorId);
        if (parent) {
          n.level = (parent.level ?? 0) + 1;
          parent.children!.push(n);
        } else {
          roots.push(n);
        }
      } else {
        roots.push(n);
      }
    });
    return roots;
  }

  private flattenTree(roots: VendorNode[]): VendorNode[] {
    const result: VendorNode[] = [];
    const walk = (nodes: VendorNode[]) => {
      for (const node of nodes) {
        result.push(node);
        if (node.expand && node.children?.length) {
          walk(node.children);
        }
      }
    };
    walk(roots);
    return result;
  }

  getVendorNameCellRenderer(params: any): string {
    const vendor = params.data as VendorNode;
    const indent = (vendor.level ?? 0) * 22;
    const hasChildren = vendor.children && vendor.children.length > 0;
    const icon = hasChildren
      ? `<i class="bi ${
          vendor.expand ? 'bi-caret-down-fill' : 'bi-caret-right-fill'
        } text-primary toggle-icon" data-id="${vendor.id}"></i>`
      : '<span style="width:16px; display:inline-block;"></span>';

    return `
      <span style="display:inline-flex; align-items:center; gap:6px; padding-left:${indent}px; cursor:pointer;">
        ${icon}
        <span class="vendor-name" data-id="${vendor.id}">${vendor.companyName ?? '—'}</span>
      </span>
    `;
  }

  private onVendorCellClick(params: any): void {
    const event = params.event as MouseEvent;
    const target = event.target as HTMLElement;

    const isToggle = target.classList.contains('toggle-icon') ||
                     target.classList.contains('vendor-name') ||
                     target.closest('.toggle-icon');

    if (!isToggle) return;

    const node = this.findNodeById(params.data.id, this.vendors);
    if (!node || !node.children?.length) return;

    node.expand = !node.expand;
    this.displayVendors = this.flattenTree(this.vendors);
    this.cdr.detectChanges();
  }

  private findNodeById(id: number, list: VendorNode[]): VendorNode | null {
    for (const n of list) {
      if (n.id === id) return n;
      const found = this.findNodeById(id, n.children ?? []);
      if (found) return found;
    }
    return null;
  }

  onAddVendor(): void {
    this.router.navigate(['/frm-vendor']);
  }

  onEditVendor(row: VendorNode): void {
    if (!row?.id) {
      this.message.warning('Please select a vendor to edit');
      return;
    }
    this.router.navigate(['/frm-vendor', row.id]);
  }

  onRefresh(): void {
    this.loadVendors();
    this.message.success('Vendor list refreshed');
  }

  onSearch(term: string): void {
    const lower = term.trim().toLowerCase();
    if (!lower) {
      this.displayVendors = this.flattenTree(this.vendors);
      return;
    }
    const filtered = this.vendors.filter(v =>
      v.companyName?.toLowerCase().includes(lower)
    );
    this.displayVendors = this.flattenTree(filtered);
  }

  onPageChange(): void {
  this.loadVendors();
}

onPageSizeChange(): void {
  this.pageNumber = 1; // reset to first page
  this.loadVendors();
}
showTotal = (total: number, range: [number, number]) =>
  `${range[0]}–${range[1]} of ${total} vendors`;





openDetail(): void {
  if (!this.selectedVendor) {
    this.message.warning("Please select a vendor to view details");
    return;
  }

  this.router.navigate(['/dtl-vendor', this.selectedVendor.id]);
}

onRowSelected(row: VendorNode) {
  this.selectedVendor = row;
}

}