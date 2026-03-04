import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/internal/operators/map';
import { environment } from '../shared/config/environment';

@Injectable({
  providedIn: 'root',
})
export class PhotoService {
  apiUrl = `${environment.apiUrl}`;

  constructor(private http: HttpClient) {}

  upload(vehicleId: number, photo: File) {
    const formData = new FormData();
    formData.append('file', photo);

    return this.http.post(`${this.apiUrl}/api/vehicles/${vehicleId}/photos`, formData);
  }

  getPhotos(vehicleId: number) {
    return this.http.get(`${this.apiUrl}/api/vehicles/${vehicleId}/photos`).pipe(map((res) => res));
  }
}
