import { Component, OnInit, ChangeDetectorRef } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClientModule, HttpErrorResponse } from "@angular/common/http";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzMessageService } from "ng-zorro-antd/message";
import { Router } from "@angular/router";
import { GridComponent } from "../../../shared/components/grid/grid.component";
import { LocationService } from "../../../shared/services/location.service";
import { Country } from "../model/country.model";

@Component({
  selector: "app-country",
  standalone: true,
  imports: [CommonModule, HttpClientModule, NzButtonModule, GridComponent],
  templateUrl: "./country.component.html",
  styleUrls: ["../../../../scss/global.css"],
})
export class CountriesComponent implements OnInit {
  /** ===================== DATA ===================== */
  zoneList: any[] = [];
  countries: any[] = [];
  filteredCountries: any[] = [];
  selectedCountry: any = null;
  isLoading = false;

  /** ===================== GRID COLUMN DEFINITIONS ===================== */
  columnDefs = [
    {
      headerName: "Flag",
      field: "flag",
      width: 100,
      cellRenderer: (params: any) =>
        params.value
          ? `<img src="${params.value}" alt="flag" width="32" height="20" style="border-radius:2px"/>`
          : "",
    },
    { headerName: "Country Name", field: "name", width: 220 },
    { headerName: "ISO2", field: "iso2", width: 100 },
    { headerName: "ISO3", field: "iso3", width: 100 },
    { headerName: "Phone Code", field: "phoneCode", width: 130 },
{
  headerName: "Zone",
  field: "zoneName",
  width: 160,
  cellRenderer: (params: any) => {
    const zone = params.value || "Unassigned";

    const isAssigned = zone !== "Unassigned";
    const badgeClass = isAssigned
      ? "badge bg-success"   
      : "badge bg-danger"; 

    return `<span class="${badgeClass}" 
                  style="padding:0.35em 0.65em; font-weight:500; font-size:0.85rem;">
              ${zone}
            </span>`;
  },
},


  ];

  constructor(
    private locationService: LocationService,
    private message: NzMessageService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadZones();
    this.loadCountries();
  }

  /** ===================== LOAD ZONES ===================== */
  loadZones(): void {
    this.locationService.getAllZones().subscribe({
      next: (res: any) => {
        const zones = Array.isArray(res.data)
          ? res.data
          : res.data?.$values || [];
        this.zoneList = zones.map((z: any) => ({
          id: z.id,
          name: z.name,
        }));
        this.cdr.detectChanges();
      },
      error: (err: HttpErrorResponse) => {
        console.error("Error loading zones:", err);
        this.zoneList = [];
        this.message.error("Failed to load zones.");
      },
    });
  }

  /** ===================== LOAD COUNTRIES ===================== */
  loadCountries(): void {
    this.isLoading = true;
    this.locationService.getAll().subscribe({
      next: (res: Country[]) => {
        this.countries = (res || []).map((c) => ({
          ...c,
          flag: c.flag
            ? c.flag
            : c.iso2
            ? `https://flagsapi.com/${c.iso2.toUpperCase()}/flat/64.png`
            : "",
          zoneName:
            this.zoneList.find((z) => z.id === c.zoneId)?.name || "Unassigned",
        }));

        this.filteredCountries = [...this.countries];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: HttpErrorResponse) => {
        console.error("Error loading countries:", err);
        this.message.error("Failed to load countries.");
        this.isLoading = false;
      },
    });
  }

  /** ===================== GRID ACTIONS ===================== */
  onAdd(): void {
    this.router.navigate(["/frmCountry"]);
  }

  onUpdate(row: any): void {
    if (!row?.id) {
      this.message.warning("Please select a country to edit.");
      return;
    }
    this.router.navigate(["/frmCountry"], {
      queryParams: { id: row.id },
    });
  }

  onRefresh(): void {
    this.loadCountries();
    this.message.success("✅ Country list refreshed successfully!");
  }

  onSearch(value: string): void {
    const term = value?.toLowerCase() || "";
    this.filteredCountries = this.countries.filter((c) =>
      c.name.toLowerCase().includes(term)
    );
    this.cdr.detectChanges();
  }

  onRowClicked(event: any): void {
    this.selectedCountry = event?.data;
  }
}
