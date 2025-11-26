import { Injectable, NgZone, OnDestroy } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable, Subscription, interval, of, tap, catchError } from "rxjs";
import { environment } from "../../../../environments/environment";

@Injectable({
  providedIn: "root",
})
export class AuthService implements OnDestroy {
  private authUrl = `${environment.apiBaseUrl}/Auth`;
  private powerUserUrl = `${environment.apiBaseUrl}/PowerUser`;

  private currentUserSubject = new BehaviorSubject<any>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  // 🔧 Configurable Settings
  private readonly inactivityLimit = 30 * 60 * 1000; // 30 minutes
  private readonly refreshInterval = 60_000; // Check every 1 min
  private readonly refreshThreshold = 5 * 60 * 1000; // Refresh if <5 min left

  private lastActivityTime = Date.now();
  private refreshSub?: Subscription;

  constructor(private http: HttpClient, private zone: NgZone) {
    this.setupActivityListener();
  }

  // ================= AUTH METHODS =================
  loginUser(username: string, password: string): Observable<any> {
    return this.http
      .post(`${this.authUrl}/login`, { username, password })
      .pipe(tap((res: any) => this.handleAuthResponse(res, "User")));
  }

  loginAdmin(username: string, password: string): Observable<any> {
    return this.http
      .post(`${this.powerUserUrl}/login`, { username, password })
      .pipe(tap((res: any) => this.handleAuthResponse(res, "Admin")));
  }

  registerPowerUser(payload: any): Observable<any> {
    return this.http.post(`${this.powerUserUrl}/Register`, payload);
  }

  private handleAuthResponse(res: any, role: string) {
    this.saveTokens(res);
    localStorage.setItem("username", res.username);
    localStorage.setItem("userId", res.userId);
    localStorage.setItem("role", role);

    this.currentUserSubject.next({ ...res, role });
    this.lastActivityTime = Date.now();
    this.restartAutoRefresh();
  }

  // ================= TOKEN HANDLING =================
  public saveTokens(res: any) {
    if (res.accessToken) localStorage.setItem("accessToken", res.accessToken);
    if (res.refreshToken) localStorage.setItem("refreshToken", res.refreshToken);
  }

  getAccessToken(): string | null {
    return localStorage.getItem("accessToken");
  }

  getRefreshToken(): string | null {
    return localStorage.getItem("refreshToken");
  }

  isLoggedIn(): boolean {
    return !!this.getAccessToken();
  }

  refreshToken(): Observable<any> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      console.warn("⚠️ No refresh token available — skipping refresh.");
      return of(null);
    }

    return this.http
      .post(`${this.authUrl}/refresh-token`, { refreshToken })
      .pipe(
        tap((res: any) => {
          console.log("✅ Token refreshed successfully.");
          this.saveTokens(res);
          this.lastActivityTime = Date.now();
        }),
        catchError((err) => {
          console.error("❌ Token refresh failed:", err);
          this.zone.run(() => {
            this.logout();
            window.location.href = "/login?m=expired";
          });
          return of(null);
        })
      );
  }

  // ================= AUTO REFRESH LOGIC =================
  private startAutoRefresh() {
    this.stopAutoRefresh();

    this.zone.runOutsideAngular(() => {
      this.refreshSub = interval(this.refreshInterval).subscribe(() => {
        const now = Date.now();

        // 💤 Check inactivity
        if (now - this.lastActivityTime >= this.inactivityLimit) {
          console.warn("⚠️ User inactive for 30 minutes. Logging out...");
          this.zone.run(() => {
            this.logout();
            window.location.href = "/login?m=timeout";
          });
          return;
        }

        // 🔄 Token refresh
        const token = this.getAccessToken();
        if (token) {
          const remaining = this.getTokenRemainingTime(token);
          if (remaining > 0 && remaining < this.refreshThreshold) {
            console.log("🔄 Refreshing token before expiry...");
            this.zone.run(() => {
              this.refreshToken().subscribe();
            });
          }
        }
      });
    });
  }

  private stopAutoRefresh() {
    this.refreshSub?.unsubscribe();
    this.refreshSub = undefined;
  }

  private restartAutoRefresh() {
    this.stopAutoRefresh();
    this.startAutoRefresh();
  }

  // ================= USER ACTIVITY =================
  private setupActivityListener() {
    const resetActivity = () => {
      this.lastActivityTime = Date.now();
    };
    ["click", "mousemove", "keydown", "scroll", "touchstart"].forEach((evt) =>
      window.addEventListener(evt, resetActivity)
    );
  }

  private getTokenRemainingTime(token: string): number {
    try {
      const payload = JSON.parse(atob(token.split(".")[1]));
      const exp = payload.exp * 1000;
      return exp - Date.now();
    } catch (e) {
      console.error("Invalid token parsing:", e);
      return 0;
    }
  }

  // ================= LOGOUT =================
  logout() {
    console.warn("🚪 Logging out user...");
    localStorage.clear();
    this.currentUserSubject.next(null);
    this.stopAutoRefresh();
  }

  ngOnDestroy() {
    this.stopAutoRefresh();
  }
}
