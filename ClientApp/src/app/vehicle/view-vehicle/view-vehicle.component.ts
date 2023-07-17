import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PhotoService } from '../../services/photo.service';
import { VehicleService } from '../../services/vehicle.service';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css']
})
export class ViewVehicleComponent implements OnInit {
  file: any; // Variable to store file
  vehicle: any;
  vehicleId: number = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private toasty: ToastrService,
    private vehicleService: VehicleService,
    private photoService: PhotoService) {

    route.params.subscribe(p => {
      this.vehicleId = +p['id'];
      if (isNaN(this.vehicleId) || this.vehicleId <= 0) {
        router.navigate(['/vehicles']);
        return;
      }
    });
  }

  // On file Select
  onChange(event: any) {
    this.file = event.target.files[0];
  }

  ngOnInit() {
    this.vehicleService.getVehicle(this.vehicleId)
      .subscribe(
        v => this.vehicle = v,
        err => {
          if (err.status == 404) {
            this.router.navigate(['/vehicles']);
            return;
          }
        });
  }

  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.delete(this.vehicle.id)
        .subscribe(x => {
          this.router.navigate(['/vehicles']);
        });
    }
  }

  uploadPhoto() {

    this.photoService.upload(this.vehicleId, this.file)
      .subscribe(x => console.log(x));
  }
}
