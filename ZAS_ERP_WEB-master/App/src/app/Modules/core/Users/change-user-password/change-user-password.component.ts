import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { UserService } from '../../../../shared/services/User.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-user-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './change-user-password.component.html',
  styleUrls: ['./change-user-password.component.css'],
})
export class ChangeUserPasswordComponent implements OnInit {
  form!: FormGroup;
  isLoading = false;
  currentUser: any;
  showOldPassword = false;
  showNewPassword = false;
  showConfirmPassword = false;
  formSubmitted = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private message: NzMessageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadCurrentUser();
  }

  /** ✅ Initialize the password form */
  private initForm(): void {
    this.form = this.fb.group(
      {
        oldPassword: ['', Validators.required],
        newPassword: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
      },
      { validators: this.passwordMatchValidator }
    );
  }

  /** ✅ Validator: ensure new password and confirm password match */
  private passwordMatchValidator(form: FormGroup) {
    const newPassword = form.get('newPassword')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return newPassword && confirmPassword && newPassword !== confirmPassword
      ? { passwordMismatch: true }
      : null;
  }

  /** ✅ Load current user just for display (not to prefill passwords) */
  private loadCurrentUser(): void {
    this.userService.getCurrentUser().subscribe({
      next: (res) => (this.currentUser = res),
      error: () => this.message.error('Failed to load user information.'),
    });
  }

  /** ✅ Handle form submit */
  submit(): void {
    this.formSubmitted = true;

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.message.warning('Please fill in all required fields correctly.');
      return;
    }

    const { oldPassword, newPassword } = this.form.value;
    const payload = { oldPassword, newPassword };

    this.isLoading = true;

    this.userService.changePassword(payload).subscribe({
      next: () => {
        this.isLoading = false;
        this.message.success('Password changed successfully! Redirecting...');

        // Clear passwords for security
        this.form.reset();

        // Redirect after short delay
        setTimeout(() => this.router.navigate(['/dashboard']), 1500);
      },
      error: (err) => {
        this.isLoading = false;
        this.message.error(err.error?.message || 'Failed to change password.');
        this.form.get('oldPassword')?.reset(); // Clear old password on failure
      },
    });
  }

  /** ✅ Cancel button action */
  onCancel(): void {
    this.router.navigate(['/dashboard']);
  }
}
