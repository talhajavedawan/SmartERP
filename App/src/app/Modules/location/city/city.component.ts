import { Component, OnInit, ChangeDetectorRef } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClientModule, HttpErrorResponse } from "@angular/common/http";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzMessageService } from "ng-zorro-antd/message";
import { NzModalModule } from "ng-zorro-antd/modal";
import { LocationService } from "../../../shared/services/location.service";
import { GetCityDto, GetStateDto } from "../model/city.model";
import { FrmCityComponent } from "./frm-city/frm-city.component";
import { Router } from "@angular/router";
import { GridLayoutComponent } from "../../../shared/components/grid-layout/grid-layout.component";

@Component({
  selector: "app-city",
  standalone: true,
  templateUrl: "./city.component.html",
  styleUrls: ["../../../../scss/global.css"],
  imports: [
    CommonModule,
    HttpClientModule,
    NzButtonModule,
    NzModalModule,
    FrmCityComponent,
    GridLayoutComponent
  ],
})
export class CityComponent implements OnInit {

  cities: GetCityDto[] = [];
  states: GetStateDto[] = [];
  selectedCity: GetCityDto | null = null;

  showModal = false;
  isEditing = false;
  editCityData?: GetCityDto;
  isLoading = false;

  isCollapsed = false;

  pageNumber = 1;
  pageSize = 20;
  totalCount = 0;

  columnDefs = [
    { field: "name", headerName: "City Name", width: 200 },
    { field: "stateName", headerName: "State", width: 200 }
  ];

  constructor(
    private svc: LocationService,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadCities();
    this.loadStates();
  }

  loadCities(): void {
    this.isLoading = true;
    this.svc.getCities(this.pageNumber, this.pageSize, "").subscribe({
      next: (res: any) => {
        const data = res?.data ?? {};
        this.cities = data.items ?? [];
        this.totalCount = data.totalCount ?? 0;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.message.error("Failed to load cities.");
      },
    });
  }

  loadStates(): void {
    this.svc.getStates("").subscribe({
      next: (res: any) => (this.states = res?.data ?? []),
      error: () => this.message.error("Failed to load states."),
    });
  }

  // ─────────────────────────────── GRID EVENTS
  onRowClicked(event: any) {
    this.selectedCity = event?.data ?? null;
  }

  onAdd() {
    this.isEditing = false;
    this.editCityData = undefined;
    this.showModal = true;
  }

  onUpdate() {
    if (!this.selectedCity)
      return void this.message.warning("Select a city to edit.");

    this.isEditing = true;
    this.editCityData = this.selectedCity;
    this.showModal = true;
  }

  onDelete() {
    if (!this.selectedCity)
      return void this.message.warning("Select a city to delete.");

    this.svc.deleteCity(this.selectedCity.id).subscribe({
      next: () => {
        this.message.success("City deleted.");
        this.loadCities();
      },
      error: () => this.message.error("Delete failed."),
    });
  }

  onSearch(value: string) {
    this.svc.getCities(1, this.pageSize, value).subscribe({
      next: (res: any) => {
        this.cities = res?.data?.items ?? [];
        this.totalCount = res?.data?.totalCount ?? 0;
      },
    });
  }

  onRefresh() {
    this.loadCities();
  }

  onPageChange(page: number) {
    this.pageNumber = page;
    this.loadCities();
  }

  onPageSizeChange(size: number) {
    this.pageSize = size;
    this.pageNumber = 1;
    this.loadCities();
  }

  // ─────────────────────────────── FORM EVENTS
  onFormSubmit() {
    this.showModal = false;
    this.loadCities();
  }

  onCancelForm() {
    this.showModal = false;
  }
}
