import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { Make } from '../shared/models/make';
import { Vehicle } from '../shared/models/vehicle';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {

  constructor(private http: HttpClient) { }

  getMakes(){
    return this.http.get<Make[]>('https://localhost:7257/api/makes').pipe(
        map(res => res ));
  }

  getFeatures(){
    return this.http.get<any[]>('https://localhost:7257/api/vehicle/features').pipe(
        map(res => res ));
  }

  create(vehicle: any){
    var data: any = {
      modelId: vehicle.modelId,
      isRegistered: vehicle.isRegistered == "true" ? true : false,
      makeId: vehicle.makeId,
      contact:{
        Name: vehicle.contact.name,
        Phone: vehicle.contact.phone,
        Email: vehicle.contact.email
    	},
      features: vehicle.features
    };

    return this.http.post('https://localhost:7257/api/vehicles',data).pipe(
        map(res => res ));
  }

  getVehicle(id: number){
    return this.http.get<any>('https://localhost:7257/api/vehicles/  ' + id).pipe(
      map(res => res ));
  }

  update(vehicle: any){
    var data: any = {
      modelId: vehicle.modelId,
      isRegistered: vehicle.isRegistered == "true" ? true : false,
      makeId: vehicle.makeId,
      contact:{
        Name: vehicle.contact.name,
        Phone: vehicle.contact.phone,
        Email: vehicle.contact.email
    	},
      features: vehicle.features
    };

    return this.http.put('https://localhost:7257/api/vehicles/' + vehicle.id ,data).pipe(
        map(res => res ));
  }

  delete(id:number){
    return this.http.delete('https://localhost:7257/api/vehicles/' + id).pipe(
        map(res => res ));
  }

  getVehicles(){

    return this.http.get<Vehicle[]>('https://localhost:7257/api/vehicles').pipe(
      map(res => res ));
  }
}
