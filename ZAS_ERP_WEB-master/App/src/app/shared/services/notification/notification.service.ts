import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, interval, switchMap, tap } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Notification, NotificationFlag, NotificationCounts } from './notification.models';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiBaseUrl}/Notification`;

  // Behavior subjects for real-time updates
  private unreadCountSubject = new BehaviorSubject<number>(0);
  private pendingCountSubject = new BehaviorSubject<number>(0);
  private urgentCountSubject = new BehaviorSubject<number>(0);
  private notificationsSubject = new BehaviorSubject<Notification[]>([]);

  // Public observables
  public unreadCount$ = this.unreadCountSubject.asObservable();
  public pendingCount$ = this.pendingCountSubject.asObservable();
  public urgentCount$ = this.urgentCountSubject.asObservable();
  public notifications$ = this.notificationsSubject.asObservable();

  constructor() {
    // Poll for new notifications every 30 seconds
    interval(30000).pipe(
      switchMap(() => this.refreshCounts())
    ).subscribe();
  }

  // Initialize - load initial counts and notifications
  public initialize(): void {
    this.refreshCounts().subscribe();
    this.getUserNotifications().subscribe();
  }

  // Refresh all counts
  public refreshCounts(): Observable<NotificationCounts> {
    return new Observable(observer => {
      Promise.all([
        this.getUnreadCount().toPromise(),
        this.getPendingCount().toPromise(),
        this.getUrgentCount().toPromise()
      ]).then(([unread, pending, urgent]) => {
        const counts: NotificationCounts = {
          unreadCount: unread?.count || 0,
          pendingCount: pending?.count || 0,
          urgentCount: urgent?.count || 0
        };
        this.unreadCountSubject.next(counts.unreadCount);
        this.pendingCountSubject.next(counts.pendingCount);
        this.urgentCountSubject.next(counts.urgentCount);
        observer.next(counts);
        observer.complete();
      }).catch(error => {
        observer.error(error);
      });
    });
  }

  // Basic CRUD operations
  getAllNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.baseUrl}`);
  }

  getNotificationById(id: number): Observable<Notification> {
    return this.http.get<Notification>(`${this.baseUrl}/${id}`);
  }

  createNotification(notification: Notification): Observable<Notification> {
    return this.http.post<Notification>(`${this.baseUrl}`, notification);
  }

  updateNotification(id: number, notification: Notification): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, notification);
  }

  deleteNotification(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  // User notifications
  getUserNotifications(take: number = 20): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.baseUrl}/user?take=${take}`).pipe(
      tap(notifications => this.notificationsSubject.next(notifications))
    );
  }

  getMoreUserNotifications(skip: number, take: number = 20): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.baseUrl}/user/more?skip=${skip}&take=${take}`);
  }

  getUserNotificationsByDate(dateFrom: Date, dateTo: Date): Observable<Notification[]> {
    return this.http.get<Notification[]>(
      `${this.baseUrl}/user/bydate?dateFrom=${dateFrom.toISOString()}&dateTo=${dateTo.toISOString()}`
    );
  }

  getSentNotificationsByDate(dateFrom: Date, dateTo: Date): Observable<Notification[]> {
    return this.http.get<Notification[]>(
      `${this.baseUrl}/sent/bydate?dateFrom=${dateFrom.toISOString()}&dateTo=${dateTo.toISOString()}`
    );
  }

  // Sent notifications
  getSentNotifications(take: number = 20): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.baseUrl}/sent?take=${take}`);
  }

  getMoreSentNotifications(skip: number, take: number = 20): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.baseUrl}/sent/more?skip=${skip}&take=${take}`);
  }

  // Unread notifications
  getUnreadNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.baseUrl}/unread`);
  }

  getUnreadCount(): Observable<{ count: number }> {
    return this.http.get<{ count: number }>(`${this.baseUrl}/unread/count`).pipe(
      tap(response => this.unreadCountSubject.next(response.count))
    );
  }

  // Pending notifications
  getPendingNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.baseUrl}/pending`);
  }

  getPendingCount(): Observable<{ count: number }> {
    return this.http.get<{ count: number }>(`${this.baseUrl}/pending/count`).pipe(
      tap(response => this.pendingCountSubject.next(response.count))
    );
  }

  // Urgent notifications
  getUrgentNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.baseUrl}/urgent`);
  }

  getUrgentCount(): Observable<{ count: number }> {
    return this.http.get<{ count: number }>(`${this.baseUrl}/urgent/count`).pipe(
      tap(response => this.urgentCountSubject.next(response.count))
    );
  }

  // Mark operations
  markAsRead(notificationId: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${notificationId}/mark-read`, {}).pipe(
      tap(() => this.refreshCounts().subscribe())
    );
  }

  markAsUnread(notificationId: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${notificationId}/mark-unread`, {}).pipe(
      tap(() => this.refreshCounts().subscribe())
    );
  }

  markMultipleAsRead(notificationIds: number[]): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/mark-read`, notificationIds).pipe(
      tap(() => this.refreshCounts().subscribe())
    );
  }

  markMultipleAsUnread(notificationIds: number[]): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/mark-unread`, notificationIds).pipe(
      tap(() => this.refreshCounts().subscribe())
    );
  }

  markAsPending(notificationIds: number[]): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/mark-pending`, notificationIds).pipe(
      tap(() => this.refreshCounts().subscribe())
    );
  }

  unmarkAsPending(notificationIds: number[]): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/unmark-pending`, notificationIds).pipe(
      tap(() => this.refreshCounts().subscribe())
    );
  }

  // Notification Flags
  getAllNotificationFlags(): Observable<NotificationFlag[]> {
    return this.http.get<NotificationFlag[]>(`${this.baseUrl}/flags`);
  }

  getActiveNotificationFlags(): Observable<NotificationFlag[]> {
    return this.http.get<NotificationFlag[]>(`${this.baseUrl}/flags/active`);
  }

  getNotificationFlagById(flagId: number): Observable<NotificationFlag> {
    return this.http.get<NotificationFlag>(`${this.baseUrl}/flags/${flagId}`);
  }

  createNotificationFlag(flag: NotificationFlag): Observable<NotificationFlag> {
    return this.http.post<NotificationFlag>(`${this.baseUrl}/flags`, flag);
  }

  updateNotificationFlag(flagId: number, flag: NotificationFlag): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/flags/${flagId}`, flag);
  }
}
