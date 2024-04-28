// data.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../config'
import { Restaurant } from '../models/restaurant.interface';


@Injectable({
  providedIn: 'root'
})
export class RestaurantService {
  constructor(private http: HttpClient) { }

  getRestaurant() {
    return this.http.get(`${API_BASE_URL}/api/data`);
  }
}



// export class ItemService {


//   constructor(private http: HttpClient) { }

//   getItems(): Observable<Item[]> {
//     return this.http.get<Item[]>(`${this.baseUrl}/api/items`);
//   }
// }
