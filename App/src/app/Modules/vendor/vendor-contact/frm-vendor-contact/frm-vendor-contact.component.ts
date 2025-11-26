import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { finalize } from 'rxjs/operators';
import { VendorContactService } from '../../../../shared/services/vendor/vendor-contact.service';
import { VendorService } from '../../../../shared/services/vendor/vendor.service';
import { LocationService } from '../../../../shared/services/location.service';

@Component({
  selector: 'app-frm-vendor-contact',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzSelectModule, NzSwitchModule, NzSpinModule],
  templateUrl: './frm-vendor-contact.component.html',
  styleUrls: ['./frm-vendor-contact.component.css'],
})
export class FrmVendorContactComponent implements OnInit {
  form!: FormGroup;
  isLoading = false;
  isEditMode = false;
  contactId!: number;
  vendorId!: number;

  vendors: any[] = [];
  countries: any[] = [];
  religions: string[] = [
    'Islam',
    'Christianity',
    'Hinduism',
    'Buddhism',
    'Sikhism',
    'Judaism',
    'Atheism',
    'Other'
  ];

  constructor(
    private fb: FormBuilder,
    private vendorContactService: VendorContactService,
    private vendorService: VendorService,
    private locationService: LocationService,
    private message: NzMessageService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadVendors();
    this.loadCountries();

this.route.queryParams.subscribe((params) => {
  this.contactId = Number(params['id']);
  this.vendorId = Number(params['vendorId']);

  if (this.contactId) {
    this.isEditMode = true;
    this.loadContact();
  }
});


    if (this.contactId) {
      this.isEditMode = true;
      this.loadContact();
    }
  }

  private initForm(): void {
    this.form = this.fb.group({
      vendorId: [null, Validators.required],
      designation: ['', [Validators.required, Validators.maxLength(100)]],
      isPrimary: [false],
      firstName: [''],
      lastName: [''],
      nationality: [null],
      religion: [null],
      phoneNumber: [''],
      email: ['', [Validators.email]],
      websiteUrl: [''],
        isActive: [true],
    });
  }

private loadVendors(): void {
  this.vendorService.getVendorDropdown().subscribe({
    next: (res) => {
      this.vendors = res.data || [];
    },
    error: () => this.message.error('Failed to load vendor dropdown list.'),
  });
}

  private loadCountries(): void {
   this.locationService.getCountries().subscribe({
      next: (res) => (this.countries = res),
      error: () => this.message.error('Failed to load country list.'),
    });
  }

  private loadContact(): void {
    this.isLoading = true;
    this.vendorContactService
      .getById(this.contactId)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: (res) => this.form.patchValue(res),
        error: () => this.message.error('Failed to load contact details.'),
        
      });
      
  }
//
submit(): void {
  if (this.form.invalid) {
    this.form.markAllAsTouched();
    this.message.warning('Please fill all required fields.');
    return;
  }

  // 🧠 Find selected vendor from the dropdown list
  const selectedVendor = this.vendors.find(v => v.id === this.form.value.vendorId);

  // 🧩 Build payload (include id when editing)
const payload: any = {
  vendorId: Number(this.form.value.vendorId),
  designation: this.form.value.designation,
  isPrimary: this.form.value.isPrimary,
  firstName: this.form.value.firstName,
  lastName: this.form.value.lastName,
  nationality: this.form.value.nationality,
  religion: this.form.value.religion,
  phoneNumber: this.form.value.phoneNumber,
  email: this.form.value.email,
  websiteUrl: this.form.value.websiteUrl,
    isActive: this.form.value.isActive,
};

// ✅ If editing, include id for TypeScript compatibility
if (this.isEditMode) {
  payload.id = this.contactId;
}



  // 🧾 Debug logs — so you can see exactly what’s going on
  console.log('🟢 isEditMode:', this.isEditMode);
  console.log('🟢 contactId:', this.contactId);
  console.log('🟢 Vendor selected:', selectedVendor);
  console.log('🟢 Payload being sent to API:', payload);

  this.isLoading = true;

  const request$ = this.isEditMode
    ? this.vendorContactService.update(this.contactId, payload)
    : this.vendorContactService.create(payload);

request$
  .pipe(finalize(() => (this.isLoading = false)))
  .subscribe({
    next: (res: any) => {
      console.log('✅ API Response:', res);

      // Show message from backend
      this.message.success(res?.message || 'Vendor contact created successfully!');

      // Optional: show new contact id in console
      console.log(`🆕 New contact created (ID: ${res?.data?.id}) for Vendor: ${res?.data?.vendorName}`);

      this.router.navigate(['/lst-vendor-contact']);
    },
    error: (err) => {
      console.error('❌ API Error:', err);
      this.message.error('Operation failed.');
    },
  });
}
compareVendors = (o1: any, o2: any): boolean =>
  o1 && o2 ? o1 === o2 || o1.id === o2.id : o1 === o2;

onVendorChange(vendorId: number): void {
  const selected = this.vendors.find(v => v.id === vendorId);
  console.log('🟢 Vendor selected:', selected?.companyName || '(none)');
}
onSearchVendor(value: string): void {
  const search = value.toLowerCase();
  this.vendors = this.vendors.filter(v =>
    v.companyName.toLowerCase().includes(search) ||
    v.ntn.toLowerCase().includes(search)
  );
}


  cancel(): void {
    this.router.navigate(['/lst-vendor-contact']);
  }
}
