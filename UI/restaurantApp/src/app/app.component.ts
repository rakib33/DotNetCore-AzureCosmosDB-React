import { HttpClient ,HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { error } from 'console';
import { response } from 'express';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'Restaurant App';
  restaurants: any;
  
  constructor(private http: HttpClient){}
  ngOnInit(): void {
   //this.getRestaurants();
   this.getAllRestaurants();
  }

  getAllRestaurants(): void {

    // Make the HTTP GET request
    //?name=${this.restaurantName}&day=${this.day}&time=${this.time}&page=${this.currentPage}&pageSize=${this.ItemPerPage}
    this.http.get<any>("https://localhost:7222/api/RestaurantDataUpload/GetRestaurants?name=''&day=''&time=''&page=1&pageSize=50")
      .subscribe(
        (response) => {
          // Handle the response data
          console.log('response :'+response);
          this.restaurants = response.data;
          console.log('response data :'+ this.restaurants);
        },
        (error) => {
          // Handle errors
          console.error('Error fetching restaurants:', error);
        }
      );
  }

}
