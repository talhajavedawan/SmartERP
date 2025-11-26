import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  FormControl
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { NzMessageService } from 'ng-zorro-antd/message';
import { LocationService } from '../../../../shared/services/location.service';
import {
  GetStateDto,
  CreateCityDto,
  UpdateCityDto
} from '../../model/city.model';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-frm-city',
  standalone: true,
  templateUrl: './frm-city.component.html',
  styleUrls: ['./frm-city.component.css'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatSelectModule,
    NgxMatSelectSearchModule
  ]
})
export class FrmCityComponent implements OnInit {
  /** Input flags */
  @Input() isEdit = false;
  @Input() cityData: any;
  @Input() states: GetStateDto[] = [];

  /** Output events */
  @Output() formSubmit = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  /** Form group and state */
  cityForm!: FormGroup;
  isLoading = false;

  /** Dropdown search filter */
  stateFilter = new FormControl('');
  filteredStates: GetStateDto[] = [];

  constructor(
    private fb: FormBuilder,
    private svc: LocationService,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.filteredStates = [...this.states];

    // 🔍 Live filtering
    this.stateFilter.valueChanges.subscribe((value) => {
      const filterValue = (value || '').toLowerCase();
      this.filteredStates = this.states.filter((s) =>
        s.name.toLowerCase().includes(filterValue)
      );
    });

    // 🧩 Pre-fill if editing
    if (this.isEdit && this.cityData) {
      this.cityForm.patchValue({
        id: this.cityData.id,
        name: this.cityData.name,
        stateId: this.cityData.stateId
      });
    }
  }

  /** Initialize the form */
  private initForm(): void {
    this.cityForm = this.fb.group({
      id: [null],
      name: ['', Validators.required],
      stateId: [null, Validators.required]
    });
  }

  /** Submit handler */
  onSubmit(): void {
    if (this.cityForm.invalid) {
      this.cityForm.markAllAsTouched();
      this.message.warning('Please fill all required fields.');
      return;
    }

    this.isLoading = true;
    const formValue = this.cityForm.value;

    if (this.isEdit) {
      // ✅ Update existing city
      const dto: UpdateCityDto = {
        id: this.cityData.id ?? formValue.id,
        name: formValue.name,
        stateId: formValue.stateId
      };

      console.log('PUT DTO =>', dto);

      this.svc.updateCity(dto.id, dto).subscribe({
        next: () => {
          this.isLoading = false;
          this.message.success('✅ City updated successfully!');
          this.formSubmit.emit();
        },
        error: (err) => {
          this.isLoading = false;
          console.error('Update error:', err);
          const msg = err?.error?.message || '❌ Failed to update city.';
          this.message.error(msg);
        }
      });
    } else {
      // ✅ Create new city
      const dto: CreateCityDto = {
        name: formValue.name,
        stateId: formValue.stateId
      };

      console.log('POST DTO =>', dto);

      this.svc.createCity(dto).subscribe({
        next: () => {
          this.isLoading = false;
          this.message.success('✅ City added successfully!');
          this.formSubmit.emit();
        },
        error: (err) => {
          this.isLoading = false;
          console.error('Create error:', err);
          const msg = err?.error?.message || '❌ Failed to add city.';
          this.message.error(msg);
        }
      });
    }
  }

  /** Cancel form */
  onCancel(): void {
    this.cancel.emit();
  }
}
