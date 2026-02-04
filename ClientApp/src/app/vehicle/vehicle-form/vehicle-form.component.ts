import * as _ from 'underscore';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { forkJoin } from 'rxjs';
import { VehicleService } from 'src/app/services/vehicle.service';
import { SaveVehicle, Vehicle } from 'src/app/shared/models/vehicle';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
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
      email: ''
    }
  };
  features: any[] = [];

  constructor(private route: ActivatedRoute,private router: Router, private vehicleService: VehicleService, private toaster: ToastrService) {

    route.params.subscribe(p=>{
      this.vehicle.id = +p['id'];
    });
  }
  ngOnInit(): void {

    var sources =[
      this.vehicleService.getMakes(),
      this.vehicleService.getFeatures(),
    ];

    if(this.vehicle.id){
      sources.push(this.vehicleService.getVehicle(this.vehicle.id))
    }

    forkJoin(sources).subscribe(data => {
      this.makes = data[0];
      this.features = data[1];
      if(this.vehicle.id){
        this.setVehicle(data[2]);
        this.populateModels();
      }
    }, err =>{
      if(err.status == 404)
        this.router.navigate(['/home'])
    })
  }

  setVehicle(v: any){
    this.vehicle.id = v.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.contact = v.contact;
    this.vehicle.features = _.pluck(v.features,'id');
  }

  onMakeChange(){
   this.populateModels();
    this.vehicle.modelId = 0;
  }

  populateModels(){
    var selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
    this.models = selectedMake? selectedMake.models : [];
  }

  onFeatureToggle(featureId: number,event: any){
    if(event.target.checked){
      this.vehicle.features.push(featureId);
    }else{
      var index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index,1);
    }
  }

  submit(){

    if(this.vehicle.id){
      this.vehicleService.update(this.vehicle).subscribe( x=>
        this.toaster.success("Success","This vehicle was sucessfully updated.")
        );
    }
    this.vehicleService.create(this.vehicle).subscribe(
      x => {
        console.log(x);
        this.toaster.success("Success", "Vehicle sucessfully created.");
      }
    );
  }

  delete(){

    if(confirm("Are you sure?")){
      this.vehicleService.delete(this.vehicle.id).subscribe( x=>
        this.router.navigate(['/home'])
        );
    }
  }
}
