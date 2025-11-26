import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import {
  ButtonModule,
  CardModule,
  FormModule,
  GridModule,
  BadgeModule,
  SpinnerModule,
  DropdownModule,
  TabsModule,
} from '@coreui/angular';
import { IconDirective } from '@coreui/icons-angular';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, GridReadyEvent, GridApi, SelectionChangedEvent } from 'ag-grid-community';
import { NotificationService } from '../../shared/services/notification/notification.service';
import { Notification, NotificationFlag } from '../../shared/services/notification/notification.models';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-notification-panel',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    ButtonModule,
    CardModule,
    FormModule,
    GridModule,
    BadgeModule,
    SpinnerModule,
    IconDirective,
    AgGridAngular,
    DropdownModule,
    TabsModule,
  ],
  templateUrl: './notification-panel.component.html',
  styleUrls: ['./notification-panel.component.scss'],
})
export class NotificationPanelComponent implements OnInit, OnDestroy {
  private notificationService = inject(NotificationService);
  private router = inject(Router);

  // Grid
  private gridApi!: GridApi;
  notifications: Notification[] = [];
  selectedNotifications: Notification[] = [];
  selectedNotification: Notification | null = null;

  // State
  loading = false;
  error: string | null = null;
  activeTab: 'inbox' | 'sent' | 'unread' | 'pending' = 'inbox';

  // Counts
  unreadCount = 0;
  pendingCount = 0;
  totalCount = 0;
  selectedCount = 0;
  filteredCount = 0;

  // Flags
  notificationFlags: NotificationFlag[] = [];
  selectedFlagId: number | null = null;

  // Date filters
  dateFrom: Date | null = null;
  dateTo: Date | null = null;

  private subscriptions: Subscription[] = [];

  // AG Grid column definitions
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
      field: 'notificationFlag.flag',
      width: 120,
      cellRenderer: (params: any) => {
        if (params.data?.notificationFlag) {
          const flag = params.data.notificationFlag;
          return `
            <div style="display: flex; align-items: center; gap: 8px;">
              <div style="width: 0; height: 0;
                border-top: 10px solid transparent;
                border-bottom: 10px solid transparent;
                border-left: 15px solid ${flag.backColor};"></div>
              <span style="font-weight: bold; color: black;">${flag.flag}</span>
            </div>
          `;
        }
        return '';
      },
    },
    {
      headerName: 'Module',
      field: 'transactionType',
      width: 120,
      valueFormatter: (params) => {
        if (!params.value) return '';
        // Check if it's already a string (from TypeScript enum) or integer (from backend)
        if (typeof params.value === 'number') {
          // Backend sends integer values
          switch(params.value) {
            case 1:
              return 'Undefined';
            case 2:
              return 'Employee';
            case 3:
              return 'Inquiry';
            case 4:
              return 'SaleOrder';
            default:
              return params.value.toString();
          }
        }
        // It's already a string value
        return params.value;
      },
    },
    {
      headerName: 'From',
      field: 'sendingUser.userName',
      width: 150,
    },
    {
      headerName: 'To',
      field: 'user.userName',
      width: 150,
    },
    {
      headerName: 'CC',
      field: 'ccUser.userName',
      width: 150,
    },
    {
      headerName: 'Title',
      field: 'title',
      width: 200,
      cellStyle: (params) => {
        return params.data?.isRead ? undefined : { fontWeight: 'bold', color: '#0072C6' };
      },
    },
    {
      headerName: 'Description',
      field: 'description',
      flex: 1,
      minWidth: 200,
    },
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
      field: 'timestamp',
      width: 180,
      valueFormatter: (params) => {
        return params.value ? new Date(params.value).toLocaleString() : '';
      },
      sort: 'desc',
    },
    {
      headerName: 'Actions',
      field: 'actions',
      width: 100,
      pinned: 'right',
      cellRenderer: (params: any) => {
        return `
          <div style="display: flex; gap: 5px; justify-content: center; align-items: center; height: 100%;">
            <svg width="16" height="16" fill="currentColor" viewBox="0 0 16 16" class="text-primary">
              <path d="M1.5 13.5L14.5 8 1.5 2.5v11z"/>
            </svg>
          </div>
        `;
      },
    },
  ];

  defaultColDef: ColDef = {
    sortable: true,
    filter: true,
    resizable: true,
    suppressMovable: true,
  };

  ngOnInit(): void {
    this.loadNotificationFlags();
    this.loadInboxNotifications();

    // Subscribe to counts
    this.subscriptions.push(
      this.notificationService.unreadCount$.subscribe(count => {
        this.unreadCount = count;
      }),
      this.notificationService.pendingCount$.subscribe(count => {
        this.pendingCount = count;
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  onGridReady(params: GridReadyEvent): void {
    this.gridApi = params.api;
    
    // Restore layout if it exists
    const savedLayout = localStorage.getItem('notificationGridLayout');
    if (savedLayout) {
      try {
        const columnState = JSON.parse(savedLayout);
        this.gridApi.applyColumnState({ state: columnState, applyOrder: true });
      } catch (e) {
        console.warn('Could not restore grid layout:', e);
      }
    }
    
    this.updateCounts();
    // Set loading to false to ensure the loading overlay is removed
    this.loading = false;
  }

  onSelectionChanged(event: SelectionChangedEvent): void {
    this.selectedNotifications = this.gridApi.getSelectedRows();
    this.selectedCount = this.selectedNotifications.length;
  }

  onRowClicked(event: any): void {
    this.selectedNotification = event.data;
  }

  onRowDoubleClicked(event: any): void {
    const notification = event.data as Notification;
    this.navigateToTransaction(notification);
  }

  onCellClicked(event: any): void {
    if (event.column.getColDef().headerName === 'Actions') {
      const notification = event.data as Notification;
      this.navigateToTransaction(notification);
    }
  }

  // Tab switching
  onTabChange(tab: 'inbox' | 'sent' | 'unread' | 'pending'): void {
    this.activeTab = tab;
    this.selectedNotification = null;
    this.selectedNotifications = [];

    switch (tab) {
      case 'inbox':
        this.loadInboxNotifications();
        break;
      case 'sent':
        this.loadSentNotifications();
        break;
      case 'unread':
        this.loadUnreadNotifications();
        break;
      case 'pending':
        this.loadPendingNotifications();
        break;
    }
  }

  // Load notifications
  loadInboxNotifications(): void {
    this.loading = true;
    this.error = null;
    this.notificationService.getUserNotifications(100).subscribe({
      next: (notifications) => {
        this.notifications = notifications;
        this.error = null;
        if (this.gridApi) {
          this.gridApi.setGridOption('rowData', this.notifications);
        }
        // Set loading to false after a small delay to ensure UI updates properly
        setTimeout(() => {
          this.loading = false;
        }, 100);
        this.updateCounts();
      },
      error: (error) => {
        console.error('Error loading inbox notifications:', error);
        this.loading = false;
        if (error.status === 401) {
          this.error = 'You must be logged in to view notifications. Please log in and try again.';
        } else {
          this.error = 'Error loading notifications. Please try again later.';
        }
      },
    });
  }

  loadSentNotifications(): void {
    this.loading = true;
    this.error = null;
    this.notificationService.getSentNotifications(100).subscribe({
      next: (notifications) => {
        this.notifications = notifications;
        this.error = null;
        if (this.gridApi) {
          this.gridApi.setGridOption('rowData', this.notifications);
        }
        setTimeout(() => {
          this.loading = false;
        }, 100);
        this.updateCounts();
      },
      error: (error) => {
        console.error('Error loading sent notifications:', error);
        this.loading = false;
        if (error.status === 401) {
          this.error = 'You must be logged in to view notifications. Please log in and try again.';
        } else {
          this.error = 'Error loading notifications. Please try again later.';
        }
      },
    });
  }

  loadUnreadNotifications(): void {
    this.loading = true;
    this.error = null;
    this.notificationService.getUnreadNotifications().subscribe({
      next: (notifications) => {
        this.notifications = notifications;
        this.error = null;
        if (this.gridApi) {
          this.gridApi.setGridOption('rowData', this.notifications);
        }
        setTimeout(() => {
          this.loading = false;
        }, 100);
        this.updateCounts();
      },
      error: (error) => {
        console.error('Error loading unread notifications:', error);
        this.loading = false;
        if (error.status === 401) {
          this.error = 'You must be logged in to view notifications. Please log in and try again.';
        } else {
          this.error = 'Error loading notifications. Please try again later.';
        }
      },
    });
  }

  loadPendingNotifications(): void {
    this.loading = true;
    this.error = null;
    this.notificationService.getPendingNotifications().subscribe({
      next: (notifications) => {
        this.notifications = notifications;
        this.error = null;
        if (this.gridApi) {
          this.gridApi.setGridOption('rowData', this.notifications);
        }
        setTimeout(() => {
          this.loading = false;
        }, 100);
        this.updateCounts();
      },
      error: (error) => {
        console.error('Error loading pending notifications:', error);
        this.loading = false;
        if (error.status === 401) {
          this.error = 'You must be logged in to view notifications. Please log in and try again.';
        } else {
          this.error = 'Error loading notifications. Please try again later.';
        }
      },
    });
  }

  loadNotificationFlags(): void {
    this.notificationService.getActiveNotificationFlags().subscribe({
      next: (flags) => {
        this.notificationFlags = flags;
      },
      error: (error) => {
        console.error('Error loading flags:', error);
      },
    });
  }

  // Actions
  markSelectedAsRead(): void {
    if (this.selectedNotifications.length === 0) return;

    const ids = this.selectedNotifications.map(n => n.id);
    this.notificationService.markMultipleAsRead(ids).subscribe({
      next: () => {
        this.refreshCurrentView();
      },
      error: (error) => console.error('Error marking as read:', error),
    });
  }

  markSelectedAsUnread(): void {
    if (this.selectedNotifications.length === 0) return;

    const ids = this.selectedNotifications.map(n => n.id);
    this.notificationService.markMultipleAsUnread(ids).subscribe({
      next: () => {
        this.refreshCurrentView();
      },
      error: (error) => console.error('Error marking as unread:', error),
    });
  }

  markSelectedAsPending(): void {
    if (this.selectedNotifications.length === 0) return;

    const ids = this.selectedNotifications.map(n => n.id);
    this.notificationService.markAsPending(ids).subscribe({
      next: () => {
        this.refreshCurrentView();
      },
      error: (error) => console.error('Error marking as pending:', error),
    });
  }

  unmarkSelectedAsPending(): void {
    if (this.selectedNotifications.length === 0) return;

    const ids = this.selectedNotifications.map(n => n.id);
    this.notificationService.unmarkAsPending(ids).subscribe({
      next: () => {
        this.refreshCurrentView();
      },
      error: (error) => console.error('Error unmarking pending:', error),
    });
  }

  loadByDateRange(): void {
    if (!this.dateFrom || !this.dateTo) {
      alert('Please select both From and To dates');
      return;
    }

    this.loading = true;
    this.error = null;
    this.notificationService.getUserNotificationsByDate(this.dateFrom, this.dateTo).subscribe({
      next: (notifications) => {
        this.notifications = notifications;
        this.loading = false;
        this.error = null;
        this.updateCounts();
      },
      error: (error) => {
        console.error('Error loading by date:', error);
        this.loading = false;
        if (error.status === 401) {
          this.error = 'You must be logged in to view notifications. Please log in and try again.';
        } else {
          this.error = 'Error loading notifications. Please try again later.';
        }
      },
    });
  }

  refreshCurrentView(): void {
    this.onTabChange(this.activeTab);
  }

  loadAllNotifications(): void {
    this.refreshCurrentView();
  }

  updateCounts(): void {
    this.totalCount = this.notifications.length;
    this.filteredCount = this.gridApi ? this.gridApi.getDisplayedRowCount() : this.totalCount;
  }

  updateFilteredCount(): void {
    if (this.gridApi) {
      this.filteredCount = this.gridApi.getDisplayedRowCount();
    }
  }

  saveLayout(): void {
    if (this.gridApi) {
      const columnState = this.gridApi.getColumnState();
      localStorage.setItem('notificationGridLayout', JSON.stringify(columnState));
      alert('Layout saved successfully!');
    }
  }

  navigateToTransaction(notification: Notification): void {
    // Mark as read first
    if (!notification.isRead) {
      this.notificationService.markAsRead(notification.id).subscribe();
    }

    // Navigate based on transaction type
    switch (notification.transactionType) {
      case 'Inquiry':
        this.router.navigate(['/inquiry-form'], {
          queryParams: { id: notification.transactionId },
        });
        break;
      case 'Employee':
        this.router.navigate(['/lst-employee']);
        break;
      case 'SaleOrder':
        // Add your sale order route
        break;
      default:
        console.log('Unknown transaction type:', notification.transactionType);
    }
  }

  loadMoreNotifications(): void {
    if (!this.dateFrom || !this.dateTo) {
      alert('Please select both From and To dates');
      return;
    }

    this.loading = true;
    this.error = null;

    // Determine which endpoint to use based on active tab
    let serviceCall;
    switch (this.activeTab) {
      case 'inbox':
        serviceCall = this.notificationService.getUserNotificationsByDate(this.dateFrom, this.dateTo);
        break;
      case 'sent':
        serviceCall = this.notificationService.getSentNotificationsByDate(this.dateFrom, this.dateTo);
        break;
      case 'unread':
        // For unread, load all user notifications in date range, then filter
        serviceCall = this.notificationService.getUserNotificationsByDate(this.dateFrom, this.dateTo);
        break;
      case 'pending':
        // For pending, load all user notifications in date range, then filter
        serviceCall = this.notificationService.getUserNotificationsByDate(this.dateFrom, this.dateTo);
        break;
      default:
        serviceCall = this.notificationService.getUserNotificationsByDate(this.dateFrom, this.dateTo);
    }

    serviceCall.subscribe({
      next: (notifications) => {
        // For unread and pending tabs, filter the results
        let filteredNotifications = notifications;
        if (this.activeTab === 'unread') {
          filteredNotifications = notifications.filter(n => !n.isRead);
        } else if (this.activeTab === 'pending') {
          filteredNotifications = notifications.filter(n => n.isPending);
        }
        
        this.notifications = filteredNotifications;
        this.error = null;
        // Reset selection count
        this.selectedCount = 0;
        if (this.gridApi) {
          // Update grid data
          this.gridApi.setGridOption('rowData', this.notifications);
        }
        // Set loading to false after a small delay to ensure UI updates properly
        setTimeout(() => {
          this.loading = false;
        }, 100);
        this.updateCounts();
      },
      error: (error) => {
        console.error('Error loading more notifications:', error);
        this.loading = false;
        if (error.status === 401) {
          this.error = 'You must be logged in to view notifications. Please log in and try again.';
        } else {
          this.error = 'Error loading notifications. Please try again later.';
        }
      }
    });
  }

  getTimeAgo(timestamp: string | Date): string {
    const date = new Date(timestamp);
    const now = new Date();
    const seconds = Math.floor((now.getTime() - date.getTime()) / 1000);

    if (seconds < 60) return 'Just now';
    if (seconds < 3600) return `${Math.floor(seconds / 60)}m ago`;
    if (seconds < 86400) return `${Math.floor(seconds / 3600)}h ago`;
    if (seconds < 604800) return `${Math.floor(seconds / 86400)}d ago`;
    return date.toLocaleDateString();
  }
}
