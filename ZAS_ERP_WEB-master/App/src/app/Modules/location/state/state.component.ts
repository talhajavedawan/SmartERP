import { Component, OnInit, ChangeDetectorRef } from "@angular/core";
import { CommonModule } from "@angular/common";
import { of, forkJoin, Observable } from "rxjs";
import { catchError } from "rxjs/operators";

import { LocationService } from "../../../shared/services/location.service";
import { CountryState, StateDto } from "../model/state.model";
import { FrmStateComponent } from "./frm-state/frm-state.component";
import { NzMessageService } from "ng-zorro-antd/message";
import { GridLayoutComponent } from "../../../shared/components/grid-layout/grid-layout.component";

@Component({
  selector: "app-state",
  standalone: true,
  templateUrl: "./state.component.html",
  styleUrls: ["./state.component.css"],
  imports: [
    CommonModule,
    FrmStateComponent,
    GridLayoutComponent
  ],
})
export class StateComponent implements OnInit {

  // 🌍 Data Lists
  countries: CountryState[] = [];
  filteredCountries: CountryState[] = [];
  states: (StateDto & { countryName?: string })[] = [];

  // 📄 Pagination
  totalStates = 0;
  currentPage = 1;
  pageSize = 20;

  // 📌 State for selected record
  selectedRowId?: number;
  selectedStateData?: StateDto;

  // 🎯 Form Control
  showModal = false;

  isEdit = false;

  // Sidebar collapse support (same as City)
  isCollapsed = false;

  constructor(
    private locationService: LocationService,
    private cdr: ChangeDetectorRef,
    private message: NzMessageService
  ) {}

  // 🔄 Init
  ngOnInit(): void {
    this.loadInitialData();
  }

  // 📌 Initial Load
  private loadInitialData(): void {
    forkJoin({
      countries: this.getCountries(),
      states: this.getStates(),
    }).subscribe(({ countries, states }) => {
      this.countries = countries;
      this.filteredCountries = countries;

      this.states = (states.data || []).map((s: any) => ({
        ...s,
        countryName: s.country?.name || ""
      }));

      this.totalStates = states.total;
      this.cdr.detectChanges();
    });
  }

  // 🌐 API HELPERS
  private getCountries(): Observable<CountryState[]> {
    return this.locationService.getCountries().pipe(
      catchError(() => of([]))
    );
  }

  private getStates(): Observable<{ data: StateDto[]; total: number }> {
    return this.locationService.getAllStates(
      "",
      this.currentPage,
      this.pageSize
    ).pipe(
      catchError(() => of({ data: [], total: 0 }))
    );
  }

  // 🔹 ROW CLICK → Select Record
  onRowClicked(event: any) {
    this.selectedRowId = event?.data?.id;
    this.selectedStateData = event?.data || null;
  }

showAddForm() {
  this.isEdit = false;
  this.selectedStateData = undefined;
  this.showModal = true;
}


editState(id: number) {
  const row = this.states.find(x => x.id === id);
  if (!row) return;

  this.isEdit = true;
  this.selectedStateData = row;
  this.showModal = true;
}


  
onFormSubmit(model: StateDto) {
  this.showModal = false;
  this.isEdit = false;
  this.selectedStateData = undefined;

  this.loadInitialData();
  this.message.success("State saved successfully!");
}



onCancelForm() {
  this.showModal = false;
  // this.isEdit = false;
  // this.selectedStateData = undefined;
}


  refreshStates() {
    this.loadInitialData();
    this.message.success("State list refreshed!");
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.loadInitialData();
  }

  onPageSizeChange(size: number) {
    this.pageSize = size;
    this.currentPage = 1;
    this.loadInitialData();
  }
}
