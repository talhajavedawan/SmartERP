import { Injectable } from "@angular/core";
import { CanActivate, Router, UrlTree } from "@angular/router";
import { AuthService } from "./auth.service";
import { JwtHelperService } from "@auth0/angular-jwt";
import { ToastrService } from "ngx-toastr";

@Injectable({
  providedIn: "root",
})
export class AuthGuard implements CanActivate {
  private jwtHelper = new JwtHelperService();

  constructor(
    private auth: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  // AuthGuard
  canActivate(): boolean {
    const token = this.auth.getAccessToken();

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }

    this.auth.logout();
    this.router.navigate(["/login"], {
      queryParams: { m: "expired" },
    });
    return false;
  }
}
