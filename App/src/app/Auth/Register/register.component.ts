import { Component } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from "@angular/forms";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { CommonModule } from "@angular/common";

import { AuthService } from "../../shared/services/authentication/auth.service";
import { CompanyService } from "../../shared/services/company-center/company/company.service";

@Component({
  selector: "app-register",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],

  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.css"],
})
export class RegisterComponent {
  showSuccess = false;
  showError = false;
  errorMessage = "";
  showPassword = false;
  showConfirmPassword = false;
  businessTypes: { id: number; businessTypeName: string }[] = [];

  adminForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private http: HttpClient,
    private companyService: CompanyService,
    private auth: AuthService
  ) {
    this.adminForm = this.fb.group(
      {
        UserName: ["", Validators.required],
        Password: ["", [Validators.required, Validators.minLength(6)]],
        ConfirmPassword: ["", Validators.required],
      },
      {
        validator: this.passwordMatchValidator,
      }
    );
  }
  passwordMatchValidator(formGroup: FormGroup) {
    const password = formGroup.get("Password");
    const confirmPassword = formGroup.get("ConfirmPassword");

    if (!password || !confirmPassword) return null;

    const passwordVal = password.value;
    const confirmPasswordVal = confirmPassword.value;

    if (passwordVal !== confirmPasswordVal) {
      confirmPassword.setErrors({
        ...(confirmPassword.errors || {}),
        mismatch: true,
      });
    } else {
      const errors = confirmPassword.errors;
      if (errors) {
        delete errors["mismatch"];
        if (Object.keys(errors).length === 0) {
          confirmPassword.setErrors(null);
        } else {
          confirmPassword.setErrors(errors);
        }
      }
    }

    return null;
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }
  register() {
    if (this.adminForm.valid) {
      const userPayload = {
        UserName: this.adminForm.value.UserName,
        Password: this.adminForm.value.Password,
        ConfirmPassword: this.adminForm.value.ConfirmPassword,
      };

      this.auth.registerPowerUser(userPayload).subscribe({
        next: () => {
          this.showSuccess = true;
          this.showError = false;
          setTimeout(() => {
            this.router.navigate(["/login"]);
          }, 1000);
        },
        error: (error) => {
          this.showError = true;
          this.showSuccess = false;
          this.errorMessage =
            error.error?.message || "User registration failed";
          setTimeout(() => (this.showError = false), 3000);
        },
      });
    } else {
      this.adminForm.markAllAsTouched();
    }
  }

  closePopup() {
    this.router.navigate(["/"]);
  }
}