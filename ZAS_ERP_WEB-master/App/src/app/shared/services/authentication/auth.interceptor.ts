import { HttpInterceptorFn, HttpErrorResponse } from "@angular/common/http";
import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { AuthService } from "./auth.service";
import { ErrorHandlerService } from "./error-handler.service";
import { catchError, switchMap, throwError } from "rxjs";

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const toastr = inject(ToastrService);
  const errorHandler = inject(ErrorHandlerService);

  let token = authService.getAccessToken();

  // ✅ Attach token if available
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      errorHandler.handle(error);
      console.log("Interceptor: Error status:", error.status, "Error:", error);

      // ✅ Skip refresh logic for login/register/refresh-token endpoints
      const isAuthRequest =
        req.url.includes("login") ||
        req.url.includes("register") ||
        req.url.includes("refresh-token");

      if (error.status === 401 && !isAuthRequest) {
        const refreshToken = authService.getRefreshToken();
        console.log("Interceptor: Refresh token:", refreshToken);

        if (refreshToken) {
          // Try refresh token
          return authService.refreshToken().pipe(
            switchMap((res: any) => {
              console.log("Interceptor: Refresh token response:", res);

              // ✅ Save new tokens
              authService.saveTokens(res);

              // ✅ Always fetch latest token from AuthService
              const newReq = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${authService.getAccessToken()}`,
                },
              });

              return next(newReq);
            }),
            catchError((refreshError) => {
              console.error("Interceptor: Refresh token failed:", refreshError);

              // Clear session + redirect
              authService.logout();
              toastr.error(
                "Session expired. Please log in again.",
                "Unauthorized"
              );
              router.navigate(["/login"], { queryParams: { m: "expired" } });
              return throwError(() => refreshError);
            })
          );
        }

        // 🚨 No refresh token → logout immediately
        console.log("Interceptor: No refresh token, logging out");
        authService.logout();
        toastr.error("You are not authorized. Please log in.", "Unauthorized");
        router.navigate(["/login"], { queryParams: { m: "unauth" } });
      }

      return throwError(() => error);
    })
  );
};
