import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { Make } from '../shared/models/make';
import { Vehicle } from '../shared/models/vehicle';
import { environment } from '../shared/config/environment';

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  private readonly vehiclesEndpoint = `${environment.apiUrl}/api/vehicles`;

  constructor(private http: HttpClient) {}

  getMakes() {
    return this.http.get<Make[]>(`${environment.apiUrl}/api/makes`).pipe(map((res) => res));
  }

  getFeatures() {
    return this.http.get<any[]>(`${environment.apiUrl}/api/features`).pipe(map((res) => res));
  }

  create(vehicle: any) {
    const data: any = {
      modelId: vehicle.modelId,
      isRegistered: vehicle.isRegistered == 'true' ? true : false,
      makeId: vehicle.makeId,
      contact: {
        Name: vehicle.contact.name,
        Phone: vehicle.contact.phone,
        Email: vehicle.contact.email,
      },
      features: vehicle.features,
    };

    return this.http.post(this.vehiclesEndpoint, data).pipe(map((res) => res));
  }

  getVehicle(id: number) {
    return this.http.get<Vehicle>(this.vehiclesEndpoint + '/' + id);
  }

  update(vehicle: any) {
    const data: any = {
      modelId: vehicle.modelId,
      isRegistered: vehicle.isRegistered == 'true' ? true : false,
      makeId: vehicle.makeId,
      contact: {
        Name: vehicle.contact.name,
        Phone: vehicle.contact.phone,
        Email: vehicle.contact.email,
      },
      features: vehicle.features,
    };

    return this.http.put(this.vehiclesEndpoint + '/' + vehicle.id, data).pipe(map((res) => res));
  }

  delete(id: number) {
    return this.http.delete(this.vehiclesEndpoint + '/' + id).pipe(map((res) => res));
  }

  getVehicles(filter: any) {
    return this.http
      .get<any[]>(this.vehiclesEndpoint + '?' + this.toQueryString(filter))
      .pipe(map((res) => res));
  }

  toQueryString(obj: any) {
    const parts = [];
    for (const property in obj) {
      const value = obj[property];

      if (value != null && value != undefined) {
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
      }
    }
    return parts.join('&');
  }
}
