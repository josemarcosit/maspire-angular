import { Component, OnInit } from '@angular/core';
import { VehicleService } from 'src/app/services/vehicle.service';
import { Make } from 'src/app/shared/models/make';
import { Model } from 'src/app/shared/models/model';
import { Vehicle } from 'src/app/shared/models/vehicle';

@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.component.html',
  styleUrls: ['./vehicle.component.css']
})
export class VehicleComponent implements OnInit {

  vehicles: Vehicle[] = [];
  makes: Make[] = [];
  models: Model[] = [];
  query: any = {
    pageSize: 3
  };

  constructor(private vehicleService: VehicleService) { }

  ngOnInit(): void {
    this.vehicleService.getMakes().subscribe(data => {
      this.makes = data;
    });

    this.populateVehicles();
  }

  private populateVehicles(){
    this.vehicleService.getVehicles(this.query).subscribe(vehicles => {
      this.vehicles = vehicles;
    });
  }

  onFilterChange(){
    this.query.modelId = 20;
    this.populateVehicles();
  }
  resetFilter(){
    this.query ={};
    this.onFilterChange();
  }

  sortBy(columnName: string){
    if(this.query.sortBy == columnName){
      this.query.isSortAscending = !this.query.isSortAscending;
    }
    else{
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }
    this.populateVehicles();
  }

  onPageChange($event: number) {
    this.query.page = $event;
    this.populateVehicles();
    }
}
