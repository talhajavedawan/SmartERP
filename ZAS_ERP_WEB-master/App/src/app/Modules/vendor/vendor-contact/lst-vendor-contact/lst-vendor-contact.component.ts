import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VendorContactService } from '../../../../shared/services/vendor/vendor-contact.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { GridLayoutComponent } from '../../../../shared/components/grid-layout/grid-layout.component';
import { ColDef } from 'ag-grid-community';
import { Router } from '@angular/router';

@Component({
  selector: 'app-lst-vendor-contact',
  standalone: true,
  imports: [CommonModule, GridLayoutComponent],
  templateUrl: './lst-vendor-contact.component.html',
  styleUrls: ['./lst-vendor-contact.component.css'],
})
export class LstVendorContactComponent implements OnInit {
  contacts: any[] = [];
  columnDefs: ColDef[] = [];
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;
  sortColumn = 'id';
  sortDirection = 'asc';
  searchTerm = '';
  selectedStatus = 'All';

  constructor(
    private vendorContactService: VendorContactService,
    private message: NzMessageService,
      private router: Router

  ) {}

  ngOnInit(): void {
    console.log('🟢 LstVendorContactComponent initialized');
    this.setupColumns();
    this.loadContacts();
  }

setupColumns(): void {
  this.columnDefs = [
    { headerName: 'Designation', field: 'designation', sortable: true },
    { headerName: 'Vendor', field: 'vendorName', sortable: true },

   {
  headerName: 'Primary',
  field: 'isPrimary',
  cellRenderer: (params: any) => {
    const isPrimary = params.value === true;
    const badgeClass = isPrimary
      ? 'badge text-bg-success fw-semibold' 
      : 'badge text-bg-secondary fw-semibold'; 
    const label = isPrimary ? 'Yes' : 'No';
    return `<span class="${badgeClass}">${label}</span>`;
  },
  cellStyle: { textAlign: 'center' },
  width: 120,
},


{
  headerName: 'Active State',
  field: 'isActive',
  cellRenderer: (params: any) => {
    const isActive = params.value === true;
    const badgeClass = isActive
      ? 'badge bg-success text-white fw-semibold'
      : 'badge bg-danger text-white fw-semibold';
    const label = isActive ? 'Active' : 'Inactive';
    return `<span class="${badgeClass}">${label}</span>`;
  },
  cellStyle: { textAlign: 'center' },
  width: 150,
},


    { headerName: 'Email', field: 'email', sortable: true },
    { headerName: 'Phone', field: 'phoneNumber', sortable: true },
    { headerName: 'Religion', field: 'religion', sortable: true },
    { headerName: 'Nationality', field: 'nationality', sortable: true },

    // ✅ Friendly labels for Created/Modified
    {
      headerName: 'Created By User',
      field: 'createdByUserName',
      sortable: true,
    },
    {
      headerName: 'Created Date',
      field: 'creationDate',
      sortable: true,
      valueFormatter: (p) =>
        p.value ? new Date(p.value).toLocaleString() : '',
    },
    {
      headerName: 'Modified By User',
      field: 'lastModifiedByUserName',
      sortable: true,
    },
    {
      headerName: 'Modified Date',
      field: 'modifiedDate',
      sortable: true,
      valueFormatter: (p) =>
        p.value ? new Date(p.value).toLocaleString() : '',
    },
  ];

  console.log(
    '🧩 Columns setup complete:',
    this.columnDefs.map((c) => c.field)
  );
}


  loadContacts(): void {
    console.log('📦 Fetching contacts with params:', {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      sortColumn: this.sortColumn,
      sortDirection: this.sortDirection,
      selectedStatus: this.selectedStatus,
    });

    this.vendorContactService
      .getAllPaged(
        this.pageNumber,
        this.pageSize,
        this.searchTerm,
        this.sortColumn,
        this.sortDirection,
        this.selectedStatus
      )
      .subscribe({
        next: (res: any) => {
          console.log('✅ Raw API Response:', res);

          // Log a sample record to inspect field names
          if (res?.data?.length > 0) {
            console.log('🧠 Sample Record:', res.data[0]);
            console.table(res.data);
          } else {
            console.warn('⚠️ API returned an empty data array.');
          }

          this.contacts = res.data;
          this.totalCount = res.totalCount;

          console.log('📊 Contacts bound to grid:', {
            count: this.contacts.length,
            totalCount: this.totalCount,
          });

          // Double-check one record to confirm the fields
          if (this.contacts.length > 0) {
            const first = this.contacts[0];
            console.log('🔍 Field Check:', {
              id: first.id,
              designation: first.designation,
              email: first.email,
              phone: first.phoneNumber,
              nationality: first.nationality,
              religion: first.religion,
            });
          }
        },
        error: (err) => {
          console.error('❌ Error loading vendor contacts:', err);
          this.message.error('Failed to load vendor contacts.');
        },
      });
  }

  onPageChange(page: number): void {
    console.log('📄 Page changed:', page);
    this.pageNumber = page;
    this.loadContacts();
  }

  onPageSizeChange(size: number): void {
    console.log('📏 Page size changed:', size);
    this.pageSize = size;
    this.pageNumber = 1;
    this.loadContacts();
  }

  onSortChange(event: { sortColumn: string; sortDirection: string }): void {
    console.log('↕️ Sort changed:', event);
    this.sortColumn = event.sortColumn;
    this.sortDirection = event.sortDirection;
    this.loadContacts();
  }

 onSearch(value: string): void {
  const trimmed = (value || '').trim();

  if (this.searchTerm === trimmed) {
    return;
  }

  console.log('🔎 Search triggered:', trimmed);

  this.searchTerm = trimmed;
  this.pageNumber = 1; // reset to first page for new search
  this.loadContacts(); // 🔄 call backend API
}

  onRefresh(): void {
    console.log('🔄 Refresh triggered');
    this.loadContacts();
  }

onAddVendorContact(): void {
  console.log('➕ Add Vendor Contact triggered');
  this.router.navigate(['/frm-vendor-contact']); 
}

// 🟠 EDIT handler
onEditVendorContact(selected: any): void {
  if (!selected || !selected.id) {
    this.message.warning('Please select a contact to edit.');
    return;
  }

  console.log('✏️ Edit Vendor Contact:', selected);
  this.router.navigate(['/frm-vendor-contact'], {
    queryParams: { id: selected.id },
  });
}

}
//