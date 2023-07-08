import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { Make } from '../shared/models/make';
import { Vehicle } from '../shared/models/vehicle';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private readonly vehiclesEndpoint = 'https://localhost:7257/api/vehicles';

  constructor(private http: HttpClient) {

  }

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

    return this.http.post(this.vehiclesEndpoint,data).pipe(
        map(res => res ));
  }

  getVehicle(id: number){
    return this.http.get<any>(this.vehiclesEndpoint + '/' + id).pipe(
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

    return this.http.put(this.vehiclesEndpoint + '/' + vehicle.id ,data).pipe(
        map(res => res ));
  }

  delete(id:number){
    return this.http.delete(this.vehiclesEndpoint + '/' + id).pipe(
        map(res => res ));
  }

  getVehicles(filter: any){

    return this.http.get<Vehicle[]>(this.vehiclesEndpoint + '?' + this.toQueryString(filter)).pipe(
      map(res => res ));
  }

  toQueryString(obj: any){
    var parts = [];
    for (var property in obj){
      var value = obj[property];

      if(value != null && value != undefined)
        parts.push(encodeURIComponent(property) + '='+ encodeURIComponent(value));
    }

    return parts.join('&');

  }
}
