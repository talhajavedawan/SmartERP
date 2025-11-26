import { Component, OnInit, OnDestroy } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { CommonModule } from "@angular/common";
import { NzMessageService } from "ng-zorro-antd/message";
import { Router, ActivatedRoute } from "@angular/router";
import { Subscription } from "rxjs";
import {
  BusinessService,
  BusinessType,
} from "../../../../../../shared/services/company-center/company/lists/business.service";
import { formatPKTDate } from "../../../../../../shared/components/dateTime.util";

@Component({
  selector: "app-frm-businesses",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: "./frm-businesses.component.html",
  styleUrls: ["../../../../../../../scss/forms.css"],
})
export class FrmBusinessesComponent implements OnInit, OnDestroy {
  businessForm: FormGroup;
  isLoading = false;
  isEdit = false;
  business: BusinessType | null = null;
  private subscriptions = new Subscription();

  formatPKTDate = formatPKTDate;

  constructor(
    private fb: FormBuilder,
    private message: NzMessageService,
    private router: Router,
    private route: ActivatedRoute,
    private businessService: BusinessService
  ) {
    this.businessForm = this.fb.group({
      businessTypeName: ["", [Validators.required, Validators.maxLength(100)]],
      isActive: [true],
    });
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.route.queryParams.subscribe((params) => {
        this.isEdit = params["isEdit"] === "true";
        const stored = sessionStorage.getItem("selectedBusiness");
        if (stored) {
          this.business = JSON.parse(stored);
          if (this.isEdit && this.business) {
            this.businessForm.patchValue({
              businessTypeName: this.business.businessTypeName,
              isActive: this.business.isActive,
            });
          }
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onSubmit(): void {
    if (this.businessForm.invalid) {
      this.markFormGroupTouched(this.businessForm);
      this.message.error("Please fill in all required fields correctly.");
      return;
    }

    this.isLoading = true;
    const payload: BusinessType = {
      id: this.isEdit && this.business?.id ? this.business.id : undefined,
      businessTypeName: this.businessForm.value.businessTypeName.trim(),
      isActive: this.businessForm.value.isActive,
    };

    // ---- uniqueness check ----
    this.businessService.getAllBusinessTypes().subscribe({
      next: (list) => {
        const duplicate = list.some(
          (x) =>
            x.businessTypeName.trim().toLowerCase() ===
              payload.businessTypeName.toLowerCase() && x.id !== payload.id
        );

        if (duplicate) {
          this.isLoading = false;
          this.message.warning("Business type name already exists.");
          return;
        }

        const action$ =
          this.isEdit && payload.id
            ? this.businessService.updateBusinessType(payload.id, payload)
            : this.businessService.createBusinessType(payload);

        action$.subscribe({
          next: () => {
            this.isLoading = false;
            this.message.success(
              this.isEdit
                ? "Business type updated successfully!"
                : "Business type added successfully!"
            );
            sessionStorage.removeItem("selectedBusiness");
            this.router.navigate(["/businesses"]);
          },
          error: () => {
            this.isLoading = false;
            this.message.error(
              this.isEdit
                ? "Failed to update business type."
                : "Failed to add business type."
            );
          },
        });
      },
      error: () => {
        this.isLoading = false;
        this.message.error("Failed to validate business type name.");
      },
    });
  }

  closeModal(): void {
    this.resetForm();
    sessionStorage.removeItem("selectedBusiness");
    this.router.navigate(["/businesses"]);
  }

  resetForm(): void {
    this.businessForm.reset({
      businessTypeName: "",
      isActive: true,
    });
    this.isLoading = false;
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach((c) => {
      c.markAsTouched();
      c.markAsDirty();
    });
  }
}
