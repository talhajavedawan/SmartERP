import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CurrencyService } from '../../../../shared/services/currency.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { CommonModule } from '@angular/common';
import { Country } from '../../model/currency.model';
import { LocationService } from '../../../../shared/services/location.service';
@Component({
  selector: 'app-frm-currency',
  templateUrl: './frm-currency.component.html',
  styleUrls: ['./frm-currency.component.css'],
  imports: [CommonModule, ReactiveFormsModule]
})
export class FrmCurrencyComponent implements OnInit {
  currencyForm: FormGroup;
  isCreateMode: boolean = true;
  isLoading: boolean = false;
  currencyId: number | null = null;
  isEditMode: boolean = false;
  countries: Country[] = []; 

  constructor(
    private fb: FormBuilder,
    private currencyService: CurrencyService,
    private countryService: LocationService,
    private router: Router,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.currencyForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(150)]],
      symbol: ['', [Validators.required]],
      abbreviation: ['', [Validators.required]],
      countryId: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.countryService.getCountries().subscribe(
      (data: Country[]) => {
        this.countries = data; 
      },
      (error) => {
        this.message.error('Error fetching countries');
      }
    );

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.currencyId = +params['id'];  
        this.isCreateMode = false;
        this.loadCurrencyData(this.currencyId); 
      }
    });
  }

  loadCurrencyData(id: number): void {
    this.isLoading = true;
    this.currencyService.getCurrencyById(id).subscribe(
      (data) => {
        this.currencyForm.patchValue(data); 
        this.isLoading = false;
      },
      (error) => {
        this.isLoading = false;
        this.message.error('Error fetching currency data');
      }
    );
  }

  onSubmit(): void {
    if (this.currencyForm.valid) {
      this.isLoading = true;
      const formData = this.currencyForm.value;

      if (this.isEditMode) {
        this.currencyService.updateCurrency(this.currencyId!, formData).subscribe(
          (response) => {
            this.isLoading = false;
            this.message.success('Currency updated successfully!');
          this.router.navigate(['/lstCurrency']);

          },
          (error) => {
            this.isLoading = false;
            this.message.error('Error updating currency');
          }
        );
      } else {
        this.currencyService.createCurrency(formData).subscribe(
          (response) => {
            this.isLoading = false;
            this.message.success('Currency created successfully!');
    this.router.navigate(['/lstCurrency']);

          },
          (error) => {
            this.isLoading = false;
            this.message.error('Error creating currency');
          }
        );
      }
    }
  }

  onReset(): void {
    this.currencyForm.reset();
    if (this.isEditMode) {
      this.loadCurrencyData(this.currencyId!); 
    }
  }

  onCancel(): void {
    this.router.navigate(['/lstCurrency']);
  }
}
