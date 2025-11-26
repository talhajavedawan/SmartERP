import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { GroupService } from '../../../../shared/services/company-center/groups/group.service';
import { Group } from '../group.model';

@Component({
  selector: 'app-frm-group',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './frm-group.component.html',
  styleUrls: ['./frm-group.component.css'],
})
export class FrmGroupComponent implements OnInit {
  /** ================== STATE ================== */
  groupForm!: FormGroup;
  isEdit = false;
  groupId: number | null = null;
  isLoading = false;

  /** ================== CONSTRUCTOR ================== */
  constructor(
    private fb: FormBuilder,
    private groupService: GroupService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {}

  /** ================== INIT ================== */
ngOnInit(): void {
  this.initializeForm();

  const id = this.route.snapshot.queryParamMap.get('id'); // ✅ FIXED
  if (id) {
    this.isEdit = true;
    this.groupId = +id;
    this.loadGroup(this.groupId);
  }
}


  /** ================== FORM SETUP ================== */
  initializeForm(): void {
    this.groupForm = this.fb.group({
      groupName: ['', [Validators.required, Validators.minLength(3)]],
      isActive: [true],
    });
  }

  /** ================== LOAD GROUP ================== */
loadGroup(id: number): void {
  this.groupService.getGroupById(id).subscribe({
    next: (group: any) => {
      this.groupForm.patchValue({
        groupName: group.groupName ?? group.GroupName,
        isActive: group.isActive ?? group.IsActive,
      });
    },
    error: () => {
      this.message.error('❌ Failed to load group.');
      this.router.navigate(['/groups']);
    },
  });
}


  /** ================== SUBMIT ================== */
submitForm(): void {
  if (this.groupForm.invalid) {
    this.groupForm.markAllAsTouched();
    return;
  }

  this.isLoading = true;

  // ✅ Attach the ID when editing
  const payload: Group = {
    ...this.groupForm.value,
    id: this.groupId ?? 0, // ensures backend ID match
  };

  const request$ = this.isEdit
    ? this.groupService.updateGroup(this.groupId!, payload)
    : this.groupService.createGroup(payload);

  request$.subscribe({
    next: () => {
      this.message.success(
        this.isEdit
          ? '✅ Group updated successfully.'
          : '✅ Group created successfully.'
      );
      this.router.navigate(['/groups']);
    },
    error: (err) => {
      console.error('Error saving group:', err);
      this.isLoading = false;
      this.message.error('❌ Failed to save group.');
    },
  });
}


  /** ================== CANCEL ================== */
  cancel(): void {
    this.router.navigate(['/groups']);
  }
}