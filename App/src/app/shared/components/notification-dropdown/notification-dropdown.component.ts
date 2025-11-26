import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  DropdownComponent,
  DropdownToggleDirective,
  DropdownMenuDirective,
  DropdownItemDirective,
  BadgeComponent
} from '@coreui/angular';
import { IconDirective } from '@coreui/icons-angular';
import { NotificationService } from '../../services/notification/notification.service';
import { Notification } from '../../services/notification/notification.models';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../../services/authentication/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-notification-dropdown',
  standalone: true,
  imports: [
    CommonModule,
    DropdownComponent,
    DropdownToggleDirective,
    DropdownMenuDirective,
    DropdownItemDirective,
    BadgeComponent,
    IconDirective
  ],
  templateUrl: './notification-dropdown.component.html',
  styleUrls: ['./notification-dropdown.component.scss']
})
export class NotificationDropdownComponent implements OnInit, OnDestroy {
  private notificationService = inject(NotificationService);
  private authService = inject(AuthService);
  private jwtHelper = inject(JwtHelperService);
  private router = inject(Router);

  notifications: Notification[] = [];
  unreadCount: number = 0;
  loading: boolean = false;

  private subscriptions: Subscription[] = [];

  ngOnInit(): void {
    // Subscribe to unread count
    this.subscriptions.push(
      this.notificationService.unreadCount$.subscribe(count => {
        this.unreadCount = count;
      })
    );

    // Subscribe to notifications
    this.subscriptions.push(
      this.notificationService.notifications$.subscribe(notifications => {
        this.notifications = notifications;
      })
    );

    // Initialize the service
    this.notificationService.initialize();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadNotifications(): void {
    // Check if user has valid authentication before loading
    const token = this.authService.getAccessToken();
    if (!token) {
      console.error('No access token available');
      this.loading = false;
      // Redirect to login if no token
      this.router.navigate(['/login']);
      return;
    }

    // Check if token is expired or close to expiration (less than 5 minutes left)
    const isTokenExpired = this.jwtHelper.isTokenExpired(token);
    const tokenExpirationDate = this.jwtHelper.getTokenExpirationDate(token);
    const timeUntilExpiration = tokenExpirationDate ? tokenExpirationDate.getTime() - Date.now() : 0;
    const isCloseToExpiration = timeUntilExpiration < 5 * 60 * 1000; // 5 minutes in milliseconds

    if (isTokenExpired || isCloseToExpiration) {
      // Try to refresh the token proactively
      this.authService.refreshToken().subscribe({
        next: (res: any) => {
          // Save new tokens
          this.authService.saveTokens(res);
          // Now load the notifications with the new token
          this.performLoadNotifications();
        },
        error: (refreshError) => {
          console.error('Token refresh failed:', refreshError);
          // If refresh fails, the interceptor should handle the redirect
          // Don't manually redirect here to avoid conflicts with interceptor
        }
      });
    } else {
      // Token is still valid, load notifications directly
      this.performLoadNotifications();
    }
  }

  private performLoadNotifications(): void {
    this.loading = true;
    this.notificationService.getUserNotifications(10).subscribe({
      next: (notifications) => {
        this.notifications = notifications;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading notifications:', error);
        this.loading = false;
        // Check if error is authentication-related (handled by interceptor)
        if (error.status === 401) {
          // Interceptor should have already redirected to login
        }
      }
    });
  }

  markAsRead(notification: Notification, event: Event): void {
    event.stopPropagation();
    if (!notification.isRead) {
      this.notificationService.markAsRead(notification.id).subscribe({
        next: () => {
          notification.isRead = true;
          this.loadNotifications();
        },
        error: (error) => console.error('Error marking as read:', error)
      });
    }
  }

  markAllAsRead(): void {
    const unreadIds = this.notifications
      .filter(n => !n.isRead)
      .map(n => n.id);

    if (unreadIds.length > 0) {
      this.notificationService.markMultipleAsRead(unreadIds).subscribe({
        next: () => {
          this.loadNotifications();
        },
        error: (error) => console.error('Error marking all as read:', error)
      });
    }
  }

  viewAllNotifications(): void {
    // Check if user has valid authentication before navigating
    const token = this.authService.getAccessToken();
    if (!token) {
      console.error('No access token available');
      this.router.navigate(['/login']);
      return;
    }

    // Check if token is expired or close to expiration
    const isTokenExpired = this.jwtHelper.isTokenExpired(token);
    const tokenExpirationDate = this.jwtHelper.getTokenExpirationDate(token);
    const timeUntilExpiration = tokenExpirationDate ? tokenExpirationDate.getTime() - Date.now() : 0;
    const isCloseToExpiration = timeUntilExpiration < 5 * 60 * 1000; // 5 minutes in milliseconds

    if (isTokenExpired || isCloseToExpiration) {
      // Try to refresh the token before navigating
      this.authService.refreshToken().subscribe({
        next: (res: any) => {
          // Save new tokens and navigate
          this.authService.saveTokens(res);
          this.router.navigate(['/notifications']);
        },
        error: (refreshError) => {
          console.error('Token refresh failed before navigation:', refreshError);
          // Let the interceptor handle the redirect
        }
      });
    } else {
      // Token is still valid, navigate directly
      this.router.navigate(['/notifications']);
    }
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

  getNotificationIcon(notification: Notification): string {
    if (notification.notificationFlag?.canGlow && notification.glow) {
      return 'cilBellExclamation';
    }
    return notification.isRead ? 'cilEnvelopeOpen' : 'cilEnvelopeClosed';
  }

  getNotificationClass(notification: Notification): string {
    const classes = [];
    if (!notification.isRead) classes.push('unread');
    if (notification.isPending) classes.push('pending');
    if (notification.glow) classes.push('urgent');
    return classes.join(' ');
  }

  onNotificationClick(notification: Notification): void {
    // Mark as read
    if (!notification.isRead) {
      this.markAsRead(notification, new Event('click'));
    }

    // Navigate based on transaction type
    switch (notification.transactionType) {
      case 'Inquiry':
        this.router.navigate(['/inquiry-form'], {
          queryParams: { id: notification.transactionId }
        });
        break;
      case 'Employee':
        this.router.navigate(['/lst-employee']);
        break;
      case 'SaleOrder':
        // Add navigation for sale orders
        break;
      default:
        console.log('Unknown transaction type:', notification.transactionType);
    }
  }
}
