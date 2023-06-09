import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { Make } from '../shared/models/make';

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
}
