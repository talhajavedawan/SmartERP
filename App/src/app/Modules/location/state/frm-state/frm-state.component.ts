import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnChanges,
  SimpleChanges,
  OnInit,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatOptionModule } from '@angular/material/core';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import {
  debounceTime,
  distinctUntilChanged,
  switchMap,
  catchError,
} from 'rxjs/operators';
import { CityState, CountryState, StateDto } from '../../model/state.model';
import { LocationService } from '../../../../shared/services/location.service';

@Component({
  selector: 'app-frm-state',
  standalone: true,
  templateUrl: './frm-state.component.html',
  styleUrls: ['./frm-state.component.css'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatOptionModule,
    NgxMatSelectSearchModule,
  ],
})
export class FrmStateComponent implements OnInit, OnChanges {
  @Input() countries: CountryState[] = [];
  @Input() filteredCountries: CountryState[] = [];
  @Input() isEdit = false;
  @Input() stateData?: StateDto;
  @Output() formSubmit = new EventEmitter<StateDto>();
  @Output() cancel = new EventEmitter<void>();

  stateForm: FormGroup;
  countryFilter = new FormControl<string | null>('');
  cityFilter = new FormControl<string | null>('');

  pagedCities: CityState[] = [];
  totalCities = 0;
  citySearch = '';
  cityPage = 1;
  cityPageSize = 20;
  isCityLoading = false;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private locationService: LocationService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.stateForm = this.fb.group({
      name: [''],
      countryId: [null],
      stateCode: [''],
      cityIds: this.fb.control<number[]>([]),
    });

    this.setupCitySearch();
  }

  ngOnInit(): void {
    this.loadCountries();
    this.setupCountrySearch();
    

    const stateId = this.route.snapshot.queryParamMap.get('id');
    if (stateId) {
      this.loadStateForEdit(Number(stateId));
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['stateData'] && this.stateData) {
      this.prefillForm(this.stateData);
    }
  }

  private setupCountrySearch(): void {
    this.countryFilter.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe((searchTerm) => {
        if (!searchTerm) {
          this.filteredCountries = this.countries;
        } else {
          const lowerTerm = searchTerm.toLowerCase();
          this.filteredCountries = this.countries.filter((c) =>
            c.name.toLowerCase().includes(lowerTerm)
          );
        }
      });
  }

  // ✅ Load countries for dropdown
  private loadCountries(): void {
    this.locationService
      .getCountries()
      .pipe(catchError(() => of<CountryState[]>([])))
      .subscribe((countries) => {
        this.countries = countries;
        this.filteredCountries = countries;
      });
  }

private loadStateForEdit(id: number): void {
  this.locationService.getStateById(id)
    .pipe(catchError(() => of(null)))
    .subscribe((res: any) => {

      const state = res?.data ?? res;
      if (!state) return;

      this.isEdit = true;

      // 🔥 STEP 1: First prefill basic fields
      this.prefillForm(state);

      // 🔥 STEP 2: Load Cities for this state’s country
      this.locationService
        .searchCities('', 1, this.cityPageSize)
        .pipe(catchError(() => of({ data: [], total: 0 })))
        .subscribe(cityRes => {

          // ALL available cities
          this.pagedCities = cityRes.data;
          this.totalCities = cityRes.total;

          this.stateForm.patchValue({
            cityIds: state.cityIds   
          });
        });

      this.stateForm.patchValue({
        countryId: state.countryId
      });
    });
}


private prefillForm(state: StateDto): void {
  this.stateForm.patchValue({
    name: state.name || '',
    countryId: state.countryId || null,
    stateCode: state.stateCode || '',
    cityIds: state.cityIds || [],
  });
}


  private setupCitySearch(): void {
    this.cityFilter.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap((search) => {
          if (!search || search.length < 2) {
            this.pagedCities = [];
            return of({ data: [], total: 0 });
          }
          this.citySearch = search;
          this.cityPage = 1;
          return this.locationService
            .searchCities(search, this.cityPage, this.cityPageSize)
            .pipe(catchError(() => of({ data: [], total: 0 })));
        })
      )
      .subscribe((res) => {
        this.pagedCities = res.data;
        this.totalCities = res.total;
      });
  }

  onCityScroll(): void {
    if (this.pagedCities.length < this.totalCities) {
      this.cityPage++;
      this.loadMoreCities();
    }
  }

  private loadMoreCities(): void {
    if (this.citySearch.length < 2) return;
    this.isCityLoading = true;
    this.locationService
      .searchCities(this.citySearch, this.cityPage, this.cityPageSize)
      .pipe(catchError(() => of({ data: [], total: 0 })))
      .subscribe((res) => {
        const newCities = res.data.filter(
          (city: CityState) =>
            !this.pagedCities.some((c: CityState) => c.id === city.id)
        );
        this.pagedCities = [...this.pagedCities, ...newCities];
        this.totalCities = res.total;
        this.isCityLoading = false;
      });
  }

  compareCities = (c1: number | null, c2: number | null): boolean => c1 === c2;

onSubmit(): void {
  if (this.stateForm.invalid) {
    this.stateForm.markAllAsTouched();
    return;
  }

  this.isLoading = true;
  const model = this.stateForm.value;
  const id = this.route.snapshot.queryParamMap.get('id');

  if (id) model.id = Number(id);

  const request = id
    ? this.locationService.updateState(Number(id), model)
    : this.locationService.createState(model);

  request.pipe(catchError(() => of(null))).subscribe(() => {
    this.isLoading = false;
    this.formSubmit.emit(model);   // 🔥 parent ko notify
  });
}




  // ❌ Cancel
  onReset(): void {
    this.stateForm.reset();
    this.stateForm.patchValue({ cityIds: [] });
this.cancel.emit();    
  }

  // 🏙️ Load initial cities when dropdown opens
  onCitySearchOpen(opened: boolean): void {
    if (opened && this.pagedCities.length === 0) {
      this.isCityLoading = true;
      this.locationService
        .searchCities('', 1, this.cityPageSize)
        .pipe(catchError(() => of({ data: [], total: 0 })))
        .subscribe((res) => {
          this.pagedCities = res.data;
          this.totalCities = res.total;
          this.isCityLoading = false;
        });
    }
  }
}
