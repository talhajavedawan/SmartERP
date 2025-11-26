import { Component, OnInit } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { CommonModule } from "@angular/common";
import { AuthService } from "../../shared/services/authentication/auth.service";

@Component({
  selector: "app-login",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent implements OnInit {
  showSuccess = false;
  showError = false;
  showUnauthorized = false; // ✅ unauthorized ka flag
  showPassword = false;
  loginForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute, // ✅ query params check karne ke liye
    private auth: AuthService
  ) {
    this.loginForm = this.fb.group({
      UserName: ["", Validators.required],
      Password: ["", Validators.required],
      IsAdmin: [false], // ✅ Checkbox or toggle
    });
  }

  ngOnInit(): void {
    // ✅ Agar /login?m=unauth ya /login?m=expired se aaye to message dikhao
    this.route.queryParamMap.subscribe((params) => {
      const m = params.get("m");
      if (m === "expired" || m === "unauth") {
        this.showUnauthorized = true;
        setTimeout(() => (this.showUnauthorized = false), 4000);
      }
    });
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  login() {
    if (this.loginForm.valid) {
      const { UserName, Password, IsAdmin } = this.loginForm.value;

      // Step 1: open window immediately (synchronously)
      const windowName = "DashboardWindow";
      const windowFeatures =
        "popup=yes,width=1200,height=800,menubar=no,toolbar=no,location=no,status=no,resizable=yes,scrollbars=yes,left=100,top=100";

      let dashboardWindow = window.open("", windowName, windowFeatures);

      if (!dashboardWindow) {
        // Popup blocked — show warning or fallback
        alert("Please allow pop-ups for this site to open the dashboard.");
        return;
      }

      // Step 2: call your API (async)
      const loginRequest = IsAdmin
        ? this.auth.loginAdmin(UserName, Password)
        : this.auth.loginUser(UserName, Password);

      loginRequest.subscribe({
        next: () => {
          const dashboardUrl = window.location.origin + "/dashboard";

          // Step 3: reuse same popup, navigate to actual dashboard
          dashboardWindow.location.href = dashboardUrl;
          dashboardWindow.focus();

          // Step 4: save a reference flag
          localStorage.setItem("dashboardWindowOpen", "true");

          this.router.navigate(["/"]);
        },
        error: () => {
          this.showError = true;
          this.showSuccess = false;
          this.showUnauthorized = false;
          setTimeout(() => (this.showError = false), 3000);

          // close the popup on failed login
          dashboardWindow.close();
        },
      });
    } else {
      this.loginForm.markAllAsTouched();
    }
  }

  closePopup() {
    this.router.navigate(["/"]);
  }
}