import { Component, OnInit, OnDestroy, Inject } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { CommonModule, DOCUMENT } from "@angular/common";
import { NzMessageService } from "ng-zorro-antd/message";
import { Router, ActivatedRoute } from "@angular/router";
import { Subscription } from "rxjs";
import {
  IndustryService,
  IndustryType,
} from "../../../../../../shared/services/company-center/company/lists/industry.service";
import { formatPKTDate } from "../../../../../../shared/components/dateTime.util";

@Component({
  selector: "app-frm-industries",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: "./frm-industries.component.html",
  styleUrls: ["../../../../../../../scss/forms.css"],
})
export class FrmIndustriesComponent implements OnInit, OnDestroy {
  industryForm: FormGroup;
  isLoading = false;
  isEdit = false;
  industry: IndustryType | null = null;
  private subscriptions = new Subscription();

  // Expose to template
  formatPKTDate = formatPKTDate;

  constructor(
    private fb: FormBuilder,
    private message: NzMessageService,
    private router: Router,
    private route: ActivatedRoute,
    private industryService: IndustryService,
    @Inject(DOCUMENT) private document: Document
  ) {
    this.industryForm = this.fb.group({
      industryTypeName: ["", [Validators.required, Validators.maxLength(100)]],
      isActive: [true],
    });
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.route.queryParams.subscribe((params) => {
        this.isEdit = params["isEdit"] === "true";
        const stored =
          this.document.defaultView?.sessionStorage.getItem("selectedIndustry");
        if (stored) {
          this.industry = JSON.parse(stored);
          if (this.isEdit && this.industry) {
            this.industryForm.patchValue({
              industryTypeName: this.industry.industryTypeName,
              isActive: this.industry.isActive,
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
    if (this.industryForm.invalid) {
      this.markFormGroupTouched(this.industryForm);
      this.message.error("Please fill in all required fields correctly.");
      return;
    }

    this.isLoading = true;
    const payload: IndustryType = {
      id: this.isEdit && this.industry?.id ? this.industry.id : undefined,
      industryTypeName: this.industryForm.value.industryTypeName.trim(),
      isActive: this.industryForm.value.isActive,
    };

    this.industryService.getAllIndustries().subscribe({
      next: (list: IndustryType[]) => {
        const duplicate = list.some(
          (x) =>
            x.industryTypeName.trim().toLowerCase() ===
              payload.industryTypeName.toLowerCase() && x.id !== payload.id
        );

        if (duplicate) {
          this.isLoading = false;
          this.message.warning("Industry name already exists.");
          return;
        }

        const action$ =
          this.isEdit && payload.id
            ? this.industryService.updateIndustry(payload.id, payload)
            : this.industryService.createIndustry(payload);

        action$.subscribe({
          next: () => {
            this.isLoading = false;
            this.message.success(
              this.isEdit
                ? "Industry updated successfully!"
                : "Industry added successfully!"
            );
            this.document.defaultView?.sessionStorage.removeItem(
              "selectedIndustry"
            );
            this.router.navigate(["/industries"]);
          },
          error: () => {
            this.isLoading = false;
            this.message.error(
              this.isEdit
                ? "Failed to update industry."
                : "Failed to add industry."
            );
          },
        });
      },
      error: () => {
        this.isLoading = false;
        this.message.error("Failed to validate industry name.");
      },
    });
  }

  closeModal(): void {
    this.resetForm();
    this.document.defaultView?.sessionStorage.removeItem("selectedIndustry");
    this.router.navigate(["/industries"]);
  }

  resetForm(): void {
    this.industryForm.reset({
      industryTypeName: "",
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
