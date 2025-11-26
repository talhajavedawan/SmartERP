import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  FormControl,
} from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { LocationService } from '../../../../shared/services/location.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-frm-zone',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    NgxMatSelectSearchModule,
  ],
  templateUrl: './frm-zone.component.html',
  styleUrls: ['./frm-zone.component.css'],
})
export class FrmZoneComponent implements OnInit, OnChanges {
  @Input() isEdit = false;
  @Input() zoneData: any;
  @Output() formSubmit = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  zoneForm!: FormGroup;
  countryList: any[] = [];
  filteredCountries: any[] = [];
  isLoading = false;
  isCountriesLoading = false;
  errorMessage = '';
  countryFilter = new FormControl('');

  constructor(
    private fb: FormBuilder,
    private svc: LocationService,
    private message: NzMessageService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  // ✅ Initialize
  ngOnInit(): void {
    this.initForm();
    this.loadCountries();

    this.route.queryParams.pipe(take(1)).subscribe((params) => {
      const id = params['id'];
      if (id) {
        this.isEdit = true;
        this.loadZoneDetails(+id);
      }
    });

    this.countryFilter.valueChanges.subscribe((val) =>
      this.filterCountries(val || '')
    );
  }

  private initForm(): void {
    this.zoneForm = this.fb.group({
      zoneName: ['', Validators.required],
      selectedCountries: [[], Validators.required],
    });
  }

  private loadCountries(): void {
    this.isCountriesLoading = true;
    this.svc.getAll().subscribe({
      next: (res: any[]) => {
        this.countryList = res || [];
        this.filteredCountries = [...this.countryList];
        this.isCountriesLoading = false;

        if (this.zoneData?.id) this.patchForm();
      },
      error: () => {
        this.isCountriesLoading = false;
        this.message.error('Failed to load countries.');
      },
    });
  }

private loadZoneDetails(id: number): void {
  this.isLoading = true;
  this.svc.getZoneById(id).subscribe({
    next: (res: any) => {
      const zone = res?.data ?? res; // ✅ unwrap
      this.zoneData = zone;
      this.isLoading = false;

      if (this.countryList.length > 0) {
        this.patchForm();
      }
    },
    error: (err) => {
      this.isLoading = false;
      console.error('Error loading zone:', err);
    },
  });
}

private patchForm(): void {
  if (!this.zoneData) return;

  const selected = this.countryList
    .filter((c) =>
      this.zoneData.countries?.some((z: any) => z.id === c.id)
    )
    .map((x) => x.id);

  this.zoneForm.patchValue({
    zoneName: this.zoneData.name || '',
    selectedCountries: selected || [],
  });
}


  filterCountries(search: string): void {
    const value = search.toLowerCase();
    this.filteredCountries = this.countryList.filter((country) =>
      country.name.toLowerCase().includes(value)
    );
  }

  onSubmit(): void {
    if (this.zoneForm.invalid) {
      this.zoneForm.markAllAsTouched();
      this.message.warning('Please fill all required fields.');
      return;
    }

    this.isLoading = true;
    const formVal = this.zoneForm.value;

    const dto = {
      name: formVal.zoneName.trim(),
      countryIds: formVal.selectedCountries,
    };

    const request = this.isEdit
      ? this.svc.updateZone(this.zoneData?.id, dto)
      : this.svc.createZone(dto);

    request.subscribe({
      next: () => {
        this.isLoading = false;
        this.message.success(
          this.isEdit ? 'Zone updated successfully!' : 'Zone added successfully!'
        );
        this.formSubmit.emit();
        this.router.navigate(['/zone']);
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Zone save error:', err);
        const msg =
          err.error?.message ||
          err.message ||
          'An unexpected error occurred.';
        this.message.error(msg);
      },
    });
  }

  onCancel(): void {
    this.cancel.emit();
    this.router.navigate(['/zone']);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes['zoneData'] &&
      this.zoneData?.id &&
      this.countryList.length > 0
    ) {
      this.patchForm();
    }
  }

  clearAllCountries(): void {
  this.zoneForm.patchValue({ selectedCountries: [] });
}

}
