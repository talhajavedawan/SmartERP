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
  StatusService,
  Status,
  TransactionItemType,
} from "../../../../../shared/services/status.service";
import { formatPKTDate } from "../../../../../shared/components/dateTime.util";
import {
  StatusClassService,
  StatusClass,
} from "../../../../../shared/services/statusClass.service";

@Component({
  selector: "app-frm-employee-status",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: "./frm-employee-status.component.html",
  styleUrls: ["../../../../../../scss/forms.css"],
})
export class FrmEmployeeStatusComponent implements OnInit, OnDestroy {
  statusForm: FormGroup;
  isLoading = false;
  isEdit = false;
  status: Status | null = null;
  statusClasses: StatusClass[] = [];
  private subs = new Subscription();

  get statusNameCtrl(): AbstractControl | null {
    return this.statusForm.get("statusName");
  }

  constructor(
    private fb: FormBuilder,
    private msg: NzMessageService,
    private router: Router,
    private route: ActivatedRoute,
    private statusSrv: StatusService,
    private statusClassSrv: StatusClassService,
    @Inject(DOCUMENT) private doc: Document
  ) {
    this.statusForm = this.fb.group({
      statusName: ["", [Validators.required, Validators.maxLength(100)]],
      backColor: [null],
      foreColor: ["#000000"],
      isActive: [true],
    });
  }

  ngOnInit(): void {
    this.subs.add(
      this.route.queryParams.subscribe((params) => {
        const id = params["id"];
        this.isEdit = !!id;

        if (this.isEdit && id) {
          this.statusSrv.getStatusById(+id).subscribe({
            next: (fresh) => {
              this.status = fresh;
              this.fillForm(fresh);
              this.loadStatusClasses(fresh.id);
            },
            error: () => this.msg.error("Failed to load status."),
          });
        } else {
          const stored = this.doc.defaultView?.sessionStorage.getItem(
            "selectedEmployeeStatus"
          );
          if (stored) {
            const parsed = JSON.parse(stored) as Status;
            this.status = parsed;
            this.fillForm(parsed);
          }
        }
      })
    );
  }

  private loadStatusClasses(statusId: number): void {
    this.statusClassSrv.getAll(TransactionItemType.Employee).subscribe({
      next: (list) => {
        // Filter only classes belonging to this Status
        this.statusClasses = list.filter((x) => x.statusId === statusId);
      },
      error: () => this.msg.error("Failed to load status class list."),
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  private fillForm(dto: Status): void {
    this.statusForm.patchValue({
      statusName: dto.statusName ?? "",
      backColor: dto.backColor ?? null,
      foreColor: dto.foreColor ?? "#000000",
      isActive: dto.isActive ?? true,
    });

    this.status = {
      ...dto,
      createdBy: dto.createdBy ?? "-",
      lastModifiedBy: dto.lastModifiedBy ?? "-",
    };
  }

  onSubmit(): void {
    if (this.statusForm.invalid) {
      this.markFormGroupTouched(this.statusForm);
      this.msg.error("Please fill all required fields.");
      return;
    }

    this.isLoading = true;

    const payload: any = {
      statusName: this.statusForm.value.statusName.trim(),
      foreColor: this.statusForm.value.foreColor,
      isActive: this.statusForm.value.isActive,
    };

    if (this.statusForm.value.backColor) {
      payload.backColor = this.statusForm.value.backColor;
    }

    if (this.isEdit && this.status?.id) {
      payload.id = this.status.id;
    }

    const request$ = this.isEdit
      ? this.statusSrv.updateStatus(
          TransactionItemType.Employee,
          this.status!.id,
          payload
        )
      : this.statusSrv.createStatus(TransactionItemType.Employee, payload);

    request$.subscribe({
      next: () => {
        this.isLoading = false;
        this.msg.success(this.isEdit ? "Updated!" : "Created!");
        this.doc.defaultView?.sessionStorage.removeItem(
          "selectedEmployeeStatus"
        );
        this.router.navigate(["/employeeStatus-list"]);
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
    this.doc.defaultView?.sessionStorage.removeItem("selectedEmployeeStatus");
    this.router.navigate(["/employeeStatus-list"]);
  }

  private resetForm(): void {
    this.statusForm.reset({
      statusName: "",
      backColor: null,
      foreColor: "#000000",
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
