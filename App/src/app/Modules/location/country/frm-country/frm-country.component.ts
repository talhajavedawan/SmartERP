import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ActivatedRoute, Router } from '@angular/router';
import { LocationService } from '../../../../shared/services/location.service';
import { CountryUpdateZoneDTO } from '../../model/country.model';

@Component({
  selector: 'app-frm-country',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzButtonModule],
  templateUrl: './frm-country.component.html',
  styleUrls: ['./frm-country.component.css']
})
export class FrmCountryComponent implements OnInit {
  countryForm!: FormGroup;
  isLoading = false;
  isStandalone = false;
  selectedCountry: any = null;
  zones: any[] = [];

  constructor(
    private fb: FormBuilder,
    private locationService: LocationService,
    private message: NzMessageService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initForm();

    this.route.queryParams.subscribe((params) => {
      const id = params['id'];
      if (id) {
        this.isStandalone = true;
        this.loadCountry(+id); // loadZones will be called inside this
      } else {
        this.loadZones(); // only for Add mode
      }
    });
  }


  

  /** 🧱 Initialize form */
  private initForm(): void {
    this.countryForm = this.fb.group({
      name: [{ value: '', disabled: true }],
      iso2: [{ value: '', disabled: true }],
      iso3: [{ value: '', disabled: true }],
      phoneCode: [{ value: '', disabled: true }],
      selectedZone: [null, Validators.required],
    });
  }

private loadCountry(id: number): void {
  this.isLoading = true;
  this.locationService.getById(id).subscribe({
    next: (res: any) => {
      console.log('✅ API Raw Country:', res);

      // ✅ Prefer ZoneId if provided, otherwise find via GetAll
      let zoneId = res.zoneId ?? res.ZoneId ?? null;

      // If still null, fetch all to match
      if (!zoneId) {
        this.locationService.getAll().subscribe((allCountries: any[]) => {
          const match = allCountries.find(c => Number(c.id) === Number(id));
          zoneId = match?.zoneId ?? match?.ZoneId ?? null;
          this.populateCountryForm(res, zoneId);
        });
      } else {
        this.populateCountryForm(res, zoneId);
      }
    },
    error: (err) => {
      this.isLoading = false;
      this.message.error('Failed to load country details.');
    }
  });
}

private populateCountryForm(res: any, zoneId: number | null): void {
  this.selectedCountry = {
    id: res.id ?? res.Id,
    name: res.name ?? res.Name,
    iso2: res.iso2 ?? res.Iso2,
    iso3: res.iso3 ?? res.Iso3,
    phoneCode: res.phoneCode ?? res.PhoneCode,
    zoneId: zoneId
  };

  this.countryForm.patchValue({
    name: this.selectedCountry.name,
    iso2: this.selectedCountry.iso2,
    iso3: this.selectedCountry.iso3,
    phoneCode: this.selectedCountry.phoneCode,
  });

  this.loadZones(zoneId);
  this.isLoading = false;
}

private loadZones(countryZoneId?: number | null): void {
  this.locationService.getAllZones().subscribe({
    next: (res: any) => {
      const data = Array.isArray(res.data) ? res.data : res.data?.$values || [];
      this.zones = data.map((z: any) => ({
        id: Number(z.id ?? z.Id),
        name: z.name ?? z.Name,
      }));

      setTimeout(() => {
        if (countryZoneId && !isNaN(countryZoneId)) {
          this.countryForm.patchValue({ selectedZone: Number(countryZoneId) });
        }
      }, 200);
    },
    error: () => this.message.error('Failed to load zones.'),
  });
}



  /** 💾 Submit (Update Zone) */
  onSubmit(): void {
    const selectedZone = this.countryForm.value.selectedZone;
    if (this.countryForm.invalid || !this.selectedCountry?.id) {
      this.message.warning('Please select a valid zone.');
      return;
    }

    const dto: CountryUpdateZoneDTO = {
      countryId: this.selectedCountry.id,
      zoneId: selectedZone,
    };

    this.isLoading = true;
    this.locationService.updateCountryZone(dto).subscribe({
      next: () => {
        this.message.success('✅ Country zone updated successfully!');
        this.isLoading = false;

        if (this.isStandalone) {
          this.router.navigate(['/country']);
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.message.error(err.error || '❌ Error updating country zone.');
      },
    });
  }

  /** ❌ Cancel */
  onCancel(): void {
    this.router.navigate(['/country']);
  }

}