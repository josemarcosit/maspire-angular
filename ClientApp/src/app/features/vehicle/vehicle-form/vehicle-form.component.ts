import * as _ from 'underscore';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { forkJoin, map, Observable } from 'rxjs';
import { VehicleService } from '../vehicle.service';
import { SaveVehicle, Vehicle } from 'src/app/core/models/vehicle';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css'],
})
export class VehicleFormComponent implements OnInit {
  makes: any[] = [];
  models: any[] = [];
  vehicle: SaveVehicle = {
    id: 0,
    modelId: 0,
    makeId: 0,
    isRegistered: false,
    features: [],
    contact: {
      name: '',
      phone: '',
      email: '',
    },
  };
  features: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private vehicleService: VehicleService,
    private toaster: ToastrService,
  ) {
    route.params.subscribe((p) => {
      this.vehicle.id = +p['id'];
    });
  }
 ngOnInit(): void {
    this.vehicle.id = +this.route.snapshot.paramMap.get('id')! || 0;

  let fork$: Observable<[any[], any[], Vehicle?]>;

  if (this.vehicle.id) {
    fork$ = forkJoin([
      this.vehicleService.getMakes(),
      this.vehicleService.getFeatures(),
      this.vehicleService.getVehicle(this.vehicle.id)
    ]) as Observable<[any[], any[], Vehicle]>;
  } else {
    fork$ = forkJoin([
      this.vehicleService.getMakes(),
      this.vehicleService.getFeatures()
    ]) as Observable<[any[], any[]]>;
  }

  fork$.subscribe((data) => {
    this.makes = data[0];
    this.features = data[1];

    if (this.vehicle.id) {
      const vehicle = data[2];
      this.setVehicle(vehicle);
      this.populateModels();
    }
  });
}

  setVehicle(v: any) {
    console.log(v);
    this.vehicle.id = v.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.contact = v.contact;
    this.vehicle.features = _.pluck(v.features, 'id');
  }

  onMakeChange() {
    this.populateModels();
    this.vehicle.modelId = 0;
  }

  populateModels() {
    let selectedMake = this.makes.find((m) => m.id == this.vehicle.makeId);
    this.models = selectedMake ? selectedMake.models : [];
  }

  onFeatureToggle(featureId: number, event: any) {
    if (event.target.checked) {
      this.vehicle.features.push(featureId);
    } else {
      const index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index, 1);
    }
  }

  submit() {
    if (this.vehicle.id) {
      // Atualizar registro existente
      this.vehicleService.update(this.vehicle)
        .subscribe(() => {
          this.toaster.success('Success', 'This vehicle was successfully updated.');
          // Opcional: redirecionar ou atualizar view
          this.router.navigate(['/vehicles', this.vehicle.id]);
        });
    } else {
      // Criar novo registro
      this.vehicleService.create(this.vehicle)
        .subscribe(() => {
          this.toaster.success('Success', 'Vehicle successfully created.');
          this.router.navigate(['/vehicles']); // ou para a listagem
        });
    }
  }

  delete() {
    if (confirm('Are you sure?')) {
      this.vehicleService.delete(this.vehicle?.id).subscribe(() => this.router.navigate(['/home']));
    }
  }
}
