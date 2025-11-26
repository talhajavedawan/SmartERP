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
  JobTitleService,
  JobTitle,
} from "../../../../../shared/services/company-center/company/lists/jobTitle.service";
import { formatPKTDate } from "../../../../../shared/components/dateTime.util";
@Component({
  selector: "app-frm-job-title",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: "./frm-job-title.component.html",
  styleUrls: ["../../../../../../scss/forms.css"],
})
export class FrmJobTitleComponent implements OnInit, OnDestroy {
  jobTitleForm: FormGroup;
  isLoading = false;
  isEdit = false;
  jobTitle: JobTitle | null = null;
  private subscriptions = new Subscription();

  constructor(
    private fb: FormBuilder,
    private message: NzMessageService,
    private router: Router,
    private route: ActivatedRoute,
    private jobTitleService: JobTitleService,
    @Inject(DOCUMENT) private document: Document
  ) {
    this.jobTitleForm = this.fb.group({
      jobTitleName: ["", [Validators.required, Validators.maxLength(100)]],
      isActive: [true],
    });
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.route.queryParams.subscribe((params) => {
        this.isEdit = params["isEdit"] === "true";
        const storedJobTitle =
          this.document.defaultView?.sessionStorage.getItem("selectedJobTitle");
        if (storedJobTitle) {
          this.jobTitle = JSON.parse(storedJobTitle);
          if (this.isEdit && this.jobTitle) {
            this.jobTitleForm.patchValue({
              jobTitleName: this.jobTitle.jobTitleName,
              isActive: this.jobTitle.isActive,
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
    if (this.jobTitleForm.invalid) {
      this.markFormGroupTouched(this.jobTitleForm);
      this.message.error("Please fill in all required fields correctly.");
      return;
    }

    this.isLoading = true;

    // Important: only send fields the API expects. Audit fields are set server-side using JWT.
    const payload: Partial<JobTitle> = {
      id: this.isEdit && this.jobTitle ? this.jobTitle.id : undefined,
      jobTitleName: this.jobTitleForm.value.jobTitleName.trim(),
      isActive: this.jobTitleForm.value.isActive,
    };

    this.jobTitleService.getAllJobTitles().subscribe({
      next: (titles: JobTitle[]) => {
        const newName = (payload.jobTitleName || "").toLowerCase();
        const alreadyExists = titles.some(
          (x) =>
            x.jobTitleName.trim().toLowerCase() === newName &&
            x.id !== payload.id
        );

        if (alreadyExists) {
          this.isLoading = false;
          this.message.warning("Job Title name already exists.");
          return;
        }

        const save$ =
          this.isEdit && payload.id
            ? this.jobTitleService.updateJobTitle(
                payload.id,
                payload as JobTitle
              )
            : this.jobTitleService.createJobTitle(payload as JobTitle);

        save$.subscribe({
          next: () => {
            this.isLoading = false;
            this.message.success(
              this.isEdit
                ? "Job Title updated successfully!"
                : "Job Title added successfully!"
            );
            this.document.defaultView?.sessionStorage.removeItem(
              "selectedJobTitle"
            );
            this.router.navigate(["/job-titles"]);
          },
          error: () => {
            this.isLoading = false;
            this.message.error(
              this.isEdit
                ? "Failed to update Job Title."
                : "Failed to add Job Title."
            );
          },
        });
      },
      error: () => {
        this.isLoading = false;
        this.message.error("Failed to validate Job Title name.");
      },
    });
  }
  formatDate(date: string | Date | undefined): string {
    return formatPKTDate(date);
  }
  closeModal(): void {
    this.resetForm();
    this.document.defaultView?.sessionStorage.removeItem("selectedJobTitle");
    this.router.navigate(["/job-titles"]);
  }

  resetForm(): void {
    this.jobTitleForm.reset({
      jobTitleName: "",
      isActive: true,
    });
    this.isLoading = false;
  }

  markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      control.markAsDirty();
    });
  }
}
