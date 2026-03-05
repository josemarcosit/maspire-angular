import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../vehicle.service';
import { LanguageService } from '../../../core/services/language.service';
import { Make } from '../../../core/models/make';
import { Model } from 'src/app/core/models/model';
import { Vehicle } from 'src/app/core/models/vehicle';

@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.component.html',
  styleUrls: ['./vehicle.component.css'],
})
export class VehicleComponent implements OnInit {
  vehicles: Vehicle[] = [];
  makes: Make[] = [];
  models: Model[] = [];
  query: any = {
    pageSize: 3,
  };

  constructor(
    private vehicleService: VehicleService,
    private languageService: LanguageService,
  ) {}

  ngOnInit(): void {
    this.vehicleService.getMakes().subscribe((data) => {
      this.makes = data;
    });

    this.populateVehicles();

    // whenever language changes, re‑run queries so Accept-Language is sent again
    this.languageService.currentLanguage$.subscribe(() => {
      this.populateVehicles();
      // also refresh makes since those labels may come from server/localized
      this.vehicleService.getMakes().subscribe((data) => (this.makes = data));
    });
  }

  private populateVehicles() {
    this.vehicleService.getVehicles(this.query).subscribe((vehicles) => {
      this.vehicles = vehicles;
    });
  }

  onFilterChange() {
    this.query.modelId = 20;
    this.populateVehicles();
  }
  resetFilter() {
    this.query = {};
    this.onFilterChange();
  }

  sortBy(columnName: string) {
    if (this.query.sortBy === columnName) {
      this.query.isSortAscending = !this.query.isSortAscending;
    } else {
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }
    this.populateVehicles();
  }

  // template compiler reports $event as Event so we accept any and coerce
  onPageChange($event: any) {
    // Pagination emits a number; if something else slips through just coerce.
    const pageNumber: number = typeof $event === 'number' ? $event : Number($event);
    this.query.page = pageNumber;
    this.populateVehicles();
  }
}
