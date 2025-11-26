import { Component, OnInit, OnDestroy, Inject } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  AbstractControl,
} from "@angular/forms";
import { CommonModule, DOCUMENT } from "@angular/common";
import { ActivatedRoute, Router } from "@angular/router";
import { NzMessageService } from "ng-zorro-antd/message";
import { Subscription } from "rxjs";
import {
  StatusClassService,
  StatusClass,
  TransactionItemType,
} from "../../../../../shared/services/statusClass.service";
import {
  StatusService,
  Status,
} from "../../../../../shared/services/status.service";
import { formatPKTDate } from "../../../../../shared/components/dateTime.util";

@Component({
  selector: "app-frm-employee-status-class",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: "./frm-employee-status-class.component.html",
  styleUrls: ["../../../../../../scss/forms.css"],
})
export class FrmEmployeeStatusClassComponent implements OnInit, OnDestroy {
  form: FormGroup;
  isLoading = false;
  isEdit = false;
  statusClass: StatusClass | null = null;
  statusList: Status[] = [];
  private subs = new Subscription();

  get classNameCtrl(): AbstractControl | null {
    return this.form.get("className");
  }

  constructor(
    private fb: FormBuilder,
    private msg: NzMessageService,
    private router: Router,
    private route: ActivatedRoute,
    private classSrv: StatusClassService,
    private statusSrv: StatusService,
    @Inject(DOCUMENT) private doc: Document
  ) {
    this.form = this.fb.group({
      className: ["", [Validators.required, Validators.maxLength(100)]],
      statusId: [null, [Validators.required]],
      foreColor: ["#000000"],
      isApproved: [false],
      isActive: [true],
    });
  }

  ngOnInit(): void {
    this.loadStatuses();

    this.subs.add(
      this.route.queryParams.subscribe((params) => {
        const id = params["id"];
        this.isEdit = !!id;

        if (this.isEdit && id) {
          this.classSrv.getById(+id).subscribe({
            next: (fresh) => {
              this.statusClass = fresh;
              this.fillForm(fresh);
            },
            error: () => this.msg.error("Failed to load status class."),
          });
        } else {
          const stored = this.doc.defaultView?.sessionStorage.getItem(
            "selectedStatusClass"
          );
          if (stored) {
            const parsed = JSON.parse(stored) as StatusClass;
            this.statusClass = parsed;
            this.fillForm(parsed);
          }
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private loadStatuses(): void {
    this.subs.add(
      this.statusSrv
        .getAllStatuses(TransactionItemType.Employee, "all")
        .subscribe({
          next: (list) => (this.statusList = list),
          error: () => this.msg.error("Failed to load statuses."),
        })
    );
  }

  private fillForm(dto: StatusClass): void {
    this.form.patchValue({
      className: dto.className ?? "",
      statusId: dto.statusId ?? null,
      foreColor: dto.foreColor ?? "#000000",
      isApproved: dto.isApproved ?? false,
      isActive: dto.isActive ?? true,
    });

    this.statusClass = {
      ...dto,
      createdBy: dto.createdBy ?? "-",
      lastModifiedBy: dto.lastModifiedBy ?? "-",
    };
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.markFormGroupTouched(this.form);
      this.msg.error("Please fill all required fields.");
      return;
    }

    this.isLoading = true;

    const payload: any = {
      className: this.form.value.className.trim(),
      statusId: this.form.value.statusId,
      foreColor: this.form.value.foreColor,
      isApproved: this.form.value.isApproved,
      isActive: this.form.value.isActive,
    };

    if (this.isEdit && this.statusClass?.id) {
      payload.id = this.statusClass.id;
    }

    const request$ = this.isEdit
      ? this.classSrv.update(
          TransactionItemType.Employee,
          this.statusClass!.id,
          payload
        )
      : this.classSrv.create(TransactionItemType.Employee, payload);

    request$.subscribe({
      next: () => {
        this.isLoading = false;
        this.msg.success(this.isEdit ? "Updated!" : "Created!");
        this.doc.defaultView?.sessionStorage.removeItem("selectedStatusClass");
        this.router.navigate(["/employeeStatusClass-list"]);
      },
      error: () => {
        this.isLoading = false;
        this.msg.error(this.isEdit ? "Update failed." : "Create failed.");
      },
    });
  }

  formatDate(date: any): string {
    if (!date) return "-";
    return formatPKTDate(date);
  }

  closeModal(): void {
    this.resetForm();
    this.doc.defaultView?.sessionStorage.removeItem("selectedStatusClass");
    this.router.navigate(["/employeeStatusClass-list"]);
  }

  private resetForm(): void {
    this.form.reset({
      className: "",
      statusId: null,
      foreColor: "#000000",
      isApproved: false,
      isActive: true,
    });
    this.isLoading = false;
  }

  private markFormGroupTouched(fg: FormGroup): void {
    Object.values(fg.controls).forEach((c) => {
      c.markAsTouched();
      c.markAsDirty();
    });
  }
}
