import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PhotoService } from '../../photo/photo.service';
import { VehicleService } from '../../vehicle/vehicle.service';
import { environment } from '../../../shared/config/environment';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css'],
})
export class ViewVehicleComponent implements OnInit {
  apiUrl = `${environment.apiUrl}`;
  activeTab: 'basic' | 'photos' = 'basic';
  file: any; // Variable to store file
  vehicle: any;
  vehicleId = 0;
  photos: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private toasty: ToastrService,
    private vehicleService: VehicleService,
    private photoService: PhotoService,
  ) {
    route.params.subscribe((p) => {
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
    this.vehicleService.getVehicle(this.vehicleId).subscribe({
      next: (v) => {
        console.log(v);
        this.vehicle = v;
      },
      error: (err) => {
        console.log(err);
      },
    });

    this.photoService.getPhotos(this.vehicleId).subscribe((photos) => (this.photos = photos));
  }

  delete() {
    if (confirm('Are you sure?')) {
      this.vehicleService.delete(this.vehicle.id).subscribe(() => {
        this.router.navigate(['/vehicles']);
      });
    }
  }

  uploadPhoto() {
    const nativeElement: any = document.querySelector('#fileInput');
    this.photoService.upload(this.vehicleId, nativeElement.files[0]).subscribe((photo) => {
      this.photos.push(photo);
    });
  }

  getImageUrl(fileName: string): string {
    return `${this.apiUrl}/uploads/${fileName}`;
  }

  selectTab(tab: 'basic' | 'photos') {
    this.activeTab = tab;
  }
}
