import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import {
  ButtonModule,
  CardModule,
  FormModule,
  GridModule,
  BadgeModule,
  SpinnerModule,
  TabsModule,
} from '@coreui/angular';
import { IconDirective } from '@coreui/icons-angular';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';

@Component({
  selector: 'app-notification-panel-demo',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    CardModule,
    FormModule,
    GridModule,
    BadgeModule,
    SpinnerModule,
    IconDirective,
    AgGridAngular,
    TabsModule,
  ],
  template: `
    <div class="notification-panel-container">
      <c-card class="h-100">
        <c-card-header class="d-flex justify-content-between align-items-center py-2">
          <h5 class="mb-0">
            <svg cIcon name="cilBell" class="me-2"></svg>
            Notification Center (Demo - Design Preview)
          </h5>
        </c-card-header>

        <c-card-body class="p-0">
          <div class="d-flex h-100">
            <!-- Main Grid Area -->
            <div class="flex-grow-1 d-flex flex-column">
              <!-- Top Row: All Tab Buttons -->
              <div class="border-bottom bg-light p-2">
                <div class="d-flex align-items-center gap-2 flex-wrap">
                  <button
                    cButton
                    [color]="activeTab === 'inbox' ? 'primary' : 'light'"
                    size="sm"
                    (click)="activeTab = 'inbox'"
                  >
                    <img src="assets/icons/notification/Inbox_16x16.png" class="me-1" style="width: 16px; height: 16px;" alt="Inbox">
                    Inbox
                  </button>

                  <button cButton color="light" size="sm">
                    <img src="assets/icons/notification/Save_16x16.png" class="me-1" style="width: 16px; height: 16px;" alt="Save">
                    Save Layout
                  </button>

                  <button
                    cButton
                    [color]="activeTab === 'unread' ? 'primary' : 'light'"
                    size="sm"
                    (click)="activeTab = 'unread'"
                  >
                    <img src="assets/icons/notification/NextComment_16x16.png" class="me-1" style="width: 16px; height: 16px;" alt="Unread">
                    Unread
                    <c-badge color="danger" shape="rounded-pill" class="ms-1">
                      3
                    </c-badge>
                  </button>

                  <button
                    cButton
                    [color]="activeTab === 'pending' ? 'primary' : 'light'"
                    size="sm"
                    (click)="activeTab = 'pending'"
                  >
                    <img src="assets/icons/notification/NextComment_16x16.png" class="me-1" style="width: 16px; height: 16px;" alt="Pending">
                    Pending
                    <c-badge color="warning" shape="rounded-pill" class="ms-1">
                      2
                    </c-badge>
                  </button>

                  <button cButton color="light" size="sm">
                    <img src="assets/icons/notification/Refresh2_16x16.png" class="me-1" style="width: 16px; height: 16px;" alt="Refresh">
                    Refresh
                  </button>

                  <button
                    cButton
                    [color]="activeTab === 'sent' ? 'primary' : 'light'"
                    size="sm"
                    (click)="activeTab = 'sent'"
                  >
                    <img src="assets/icons/notification/Outbox_16x16.png" class="me-1" style="width: 16px; height: 16px;" alt="Sent">
                    Sent
                  </button>
                </div>
              </div>

              <!-- AG Grid -->
              <div class="flex-grow-1">
                <ag-grid-angular
                  class="ag-theme-alpine"
                  style="width: 100%; height: 100%"
                  [rowData]="mockNotifications"
                  [columnDefs]="columnDefs"
                  [defaultColDef]="defaultColDef"
                  [rowSelection]="'multiple'"
                  [animateRows]="true"
                  (rowClicked)="onRowClicked($event)"
                >
                </ag-grid-angular>
              </div>

              <!-- Action Buttons Row 1 (BELOW GRID) -->
              <div class="border-top bg-light p-2">
                <div class="d-flex align-items-center gap-2 flex-wrap">
                  <button cButton color="success" size="sm">
                    <img src="assets/icons/notification/CheckBox_32x32.png" class="me-1" style="width: 18px; height: 18px;" alt="Mark Read">
                    Mark Read
                  </button>

                  <button cButton color="info" size="sm" [disabled]="true">
                    Count: {{ mockNotifications.length }}
                  </button>

                  <button cButton color="info" size="sm" [disabled]="true">
                    <img src="assets/icons/notification/CheckBox2_16x16.png" class="me-1" style="width: 16px; height: 16px;" alt="Selected">
                    Selected: 0
                  </button>

                  <button cButton color="info" size="sm" [disabled]="true">
                    <img src="assets/icons/notification/CheckBox2_16x16.png" class="me-1" style="width: 16px; height: 16px;" alt="Filtered">
                    Filtered: {{ mockNotifications.length }}
                  </button>

                  <!-- Date Filters -->
                  <div class="d-flex align-items-center gap-2 ms-auto">
                    <input
                      type="date"
                      class="form-control form-control-sm"
                      style="width: 140px"
                      placeholder="From"
                    />
                    <input
                      type="date"
                      class="form-control form-control-sm"
                      style="width: 140px"
                      placeholder="To"
                    />
                    <button cButton color="primary" size="sm">
                      Load
                    </button>
                    <button cButton color="primary" size="sm">
                      Load All
                    </button>
                  </div>

                  <button cButton color="warning" size="sm">
                    <img src="assets/icons/notification/CheckBox_32x32.png" class="me-1" style="width: 18px; height: 18px;" alt="Mark Unread">
                    Mark Unread
                  </button>
                </div>
              </div>

              <!-- Action Buttons Row 2 (BELOW ROW 1) -->
              <div class="border-top bg-light p-2">
                <div class="d-flex align-items-center gap-2">
                  <button cButton color="warning" size="sm">
                    <img src="assets/icons/notification/CheckBox_32x32.png" class="me-1" style="width: 18px; height: 18px;" alt="Mark Pending">
                    Mark Pending
                  </button>

                  <button cButton color="secondary" size="sm">
                    <img src="assets/icons/notification/CheckBox_32x32.png" class="me-1" style="width: 18px; height: 18px;" alt="Unmark Pending">
                    Unmark Pending
                  </button>
                </div>
              </div>
            </div>

            <!-- Right Side Detail Panel -->
            <div class="notification-detail-panel border-start">
              @if (selectedNotification) {
              <div class="h-100 d-flex flex-column">
                <!-- Close Button -->
                <div class="d-flex justify-content-end p-2 border-bottom">
                  <button
                    cButton
                    color="light"
                    size="sm"
                    (click)="selectedNotification = null"
                  >
                    <svg cIcon name="cilX"></svg>
                  </button>
                </div>

                <!-- Notification Details -->
                <div class="flex-grow-1 p-3 overflow-auto">
                  <!-- User Info -->
                  <div class="text-center mb-3">
                    <div class="user-avatar mb-2">
                      <svg cIcon name="cilUser" size="3xl" class="text-primary"></svg>
                    </div>
                    <h6 class="mb-1">{{ selectedNotification.from }}</h6>
                    <small class="text-muted">{{ selectedNotification.received }}</small>
                  </div>

                  <!-- Flag -->
                  <div class="mb-3">
                    <label class="form-label fw-bold">Flag:</label>
                    <div>
                      <c-badge [style.background-color]="selectedNotification.flagColor">
                        {{ selectedNotification.flag }}
                      </c-badge>
                    </div>
                  </div>

                  <!-- Module -->
                  <div class="mb-3">
                    <label class="form-label fw-bold">Module:</label>
                    <div>{{ selectedNotification.module }}</div>
                  </div>

                  <!-- Title -->
                  <div class="mb-3">
                    <label class="form-label fw-bold">Title:</label>
                    <div>{{ selectedNotification.title }}</div>
                  </div>

                  <!-- Description -->
                  <div class="mb-3">
                    <label class="form-label fw-bold">Description:</label>
                    <div class="notification-description">
                      {{ selectedNotification.description }}
                    </div>
                  </div>
                </div>

                <!-- Action Buttons -->
                <div class="p-3 border-top">
                  <button cButton color="primary" class="w-100 mb-2">
                    <svg cIcon name="cilArrowRight" class="me-1"></svg>
                    Open Transaction
                  </button>

                  <button cButton color="success" class="w-100">
                    <svg cIcon name="cilCheckCircle" class="me-1"></svg>
                    Mark as Read
                  </button>
                </div>
              </div>
              } @else {
              <div class="h-100 d-flex align-items-center justify-content-center text-muted">
                <div class="text-center">
                  <svg cIcon name="cilCursor" size="3xl" class="mb-3"></svg>
                  <p>Select a notification to view details</p>
                </div>
              </div>
              }
            </div>
          </div>
        </c-card-body>
      </c-card>
    </div>
  `,
  styles: [`
    .notification-panel-container {
      height: calc(100vh - 120px);
      padding: 1rem;
    }

    .notification-panel-container c-card {
      height: 100%;
    }

    .notification-panel-container c-card-body {
      height: calc(100% - 60px);
      overflow: hidden;
    }

    .notification-detail-panel {
      width: 350px;
      flex-shrink: 0;
      background-color: var(--cui-body-bg, #f8f9fa);
    }

    .user-avatar {
      width: 80px;
      height: 80px;
      border-radius: 50%;
      background-color: var(--cui-secondary-bg, #e9ecef);
      display: flex;
      align-items: center;
      justify-content: center;
      margin: 0 auto;
    }

    .notification-description {
      background-color: var(--cui-body-bg, white);
      padding: 0.75rem;
      border-radius: 6px;
      border: 1px solid var(--cui-border-color, #dee2e6);
      color: var(--cui-body-color, inherit);
      white-space: pre-wrap;
      word-wrap: break-word;
    }

    :host ::ng-deep .ag-theme-alpine {
      --ag-font-size: 14px;
      --ag-background-color: var(--cui-body-bg, #fff);
      --ag-foreground-color: var(--cui-body-color, #000);
      --ag-border-color: var(--cui-border-color, #dee2e6);
      --ag-header-background-color: var(--cui-tertiary-bg, #f8f9fa);
      --ag-header-foreground-color: var(--cui-body-color, #000);
      --ag-odd-row-background-color: var(--cui-body-bg, #fff);
      --ag-row-hover-color: var(--cui-secondary-bg, #e9ecef);
    }
  `]
})
export class NotificationPanelDemoComponent implements OnInit {
  activeTab: 'inbox' | 'sent' | 'unread' | 'pending' = 'inbox';
  selectedNotification: any = null;

  mockNotifications = [
    {
      flag: 'Urgent',
      flagColor: '#DC3545',
      module: 'Inquiry',
      from: 'John Smith',
      to: 'You',
      cc: 'Jane Doe',
      title: 'New Inquiry Received',
      description: 'Customer ABC Corp has submitted a new inquiry requiring immediate attention.',
      isPending: false,
      received: '5 minutes ago'
    },
    {
      flag: 'Important',
      flagColor: '#FFC107',
      module: 'Employee',
      from: 'Mary Johnson',
      to: 'You',
      cc: '',
      title: 'Employee Update Required',
      description: 'Please review and approve employee information changes.',
      isPending: true,
      received: '2 hours ago'
    },
    {
      flag: 'Normal',
      flagColor: '#0D6EFD',
      module: 'Sale Order',
      from: 'Mike Wilson',
      to: 'You',
      cc: 'Sales Team',
      title: 'New Sale Order Created',
      description: 'Sale order #SO-2024-001 has been created and awaits your approval.',
      isPending: false,
      received: '1 day ago'
    },
    {
      flag: 'Urgent',
      flagColor: '#DC3545',
      module: 'Inquiry',
      from: 'Sarah Davis',
      to: 'You',
      cc: '',
      title: 'Urgent: Customer Follow-up',
      description: 'Customer XYZ Industries requires immediate response on inquiry #INQ-2024-002.',
      isPending: true,
      received: '3 hours ago'
    },
    {
      flag: 'Low Priority',
      flagColor: '#6C757D',
      module: 'Employee',
      from: 'Tom Brown',
      to: 'You',
      cc: 'HR Department',
      title: 'Monthly Report Available',
      description: 'The monthly employee performance report is now available for review.',
      isPending: false,
      received: '2 days ago'
    }
  ];

  columnDefs: ColDef[] = [
    {
      headerName: '',
      checkboxSelection: true,
      headerCheckboxSelection: true,
      width: 50,
      pinned: 'left',
    },
    {
      headerName: 'Flag',
      field: 'flag',
      width: 120,
      headerComponent: (params: any) => {
        const eHeader = document.createElement('div');
        eHeader.innerHTML = `<img src="assets/icons/notification/Status_16x16.png" style="width: 14px; height: 14px; margin-right: 5px; vertical-align: middle;">Flag`;
        return eHeader;
      },
      cellRenderer: (params: any) => {
        if (params.data) {
          return `
            <div style="display: flex; align-items: center; gap: 8px;">
              <div style="width: 0; height: 0;
                border-top: 10px solid transparent;
                border-bottom: 10px solid transparent;
                border-left: 15px solid ${params.data.flagColor};"></div>
              <span style="font-weight: bold;">${params.data.flag}</span>
            </div>
          `;
        }
        return '';
      },
    },
    {
      headerName: 'Module',
      field: 'module',
      width: 120,
      headerComponent: (params: any) => {
        const eHeader = document.createElement('div');
        eHeader.innerHTML = `<img src="assets/icons/notification/BOTask_16x16.png" style="width: 14px; height: 14px; margin-right: 5px; vertical-align: middle;">Module`;
        return eHeader;
      },
    },
    {
      headerName: 'From',
      field: 'from',
      width: 150,
      headerComponent: (params: any) => {
        const eHeader = document.createElement('div');
        eHeader.innerHTML = `<img src="assets/icons/notification/User_16x16.png" style="width: 14px; height: 14px; margin-right: 5px; vertical-align: middle;">From`;
        return eHeader;
      },
    },
    {
      headerName: 'To',
      field: 'to',
      width: 150,
      headerComponent: (params: any) => {
        const eHeader = document.createElement('div');
        eHeader.innerHTML = `<img src="assets/icons/notification/User_16x16.png" style="width: 14px; height: 14px; margin-right: 5px; vertical-align: middle;">To`;
        return eHeader;
      },
    },
    {
      headerName: 'CC',
      field: 'cc',
      width: 150,
      headerComponent: (params: any) => {
        const eHeader = document.createElement('div');
        eHeader.innerHTML = `<img src="assets/icons/notification/User_16x16.png" style="width: 14px; height: 14px; margin-right: 5px; vertical-align: middle;">CC`;
        return eHeader;
      },
    },
    {
      headerName: 'Title',
      field: 'title',
      width: 200,
      headerComponent: (params: any) => {
        const eHeader = document.createElement('div');
        eHeader.innerHTML = `<img src="assets/icons/notification/Edit_16x16.png" style="width: 14px; height: 14px; margin-right: 5px; vertical-align: middle;">Title`;
        return eHeader;
      },
      cellStyle: { fontWeight: 'bold', color: '#0072C6' }
    },
    { headerName: 'Description', field: 'description', flex: 1, minWidth: 200 },
    {
      headerName: 'Is Pending',
      field: 'isPending',
      width: 100,
      cellRenderer: (params: any) => {
        return params.value ? '<span class="badge bg-warning">Pending</span>' : '';
      },
    },
    {
      headerName: 'Received',
      field: 'received',
      width: 180,
      headerComponent: (params: any) => {
        const eHeader = document.createElement('div');
        eHeader.innerHTML = `<img src="assets/icons/notification/Time_16x16.png" style="width: 14px; height: 14px; margin-right: 5px; vertical-align: middle;">Received`;
        return eHeader;
      },
    },
  ];

  defaultColDef: ColDef = {
    sortable: true,
    filter: true,
    resizable: true,
  };

  ngOnInit(): void {
    console.log('Demo notification panel loaded with', this.mockNotifications.length, 'mock notifications');
  }

  onRowClicked(event: any): void {
    this.selectedNotification = event.data;
  }
}
