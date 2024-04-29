using Moq;
using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RestaurantOpeningApi.Test.ControllerTest
{
    public class RestaurantControllerTest
    {
        //[Fact]
        //public async void RestaurantController_GetRestaurants_Test()
        //{
        //    //Arrange
        //    var mockRestaurantService = new Mock<IRestaurantDataService>();
        //    var parameters = new RestaurantParameters();
        //    parameters.Pagination.Page = 1;
        //    parameters.Pagination.PageSize = 10;

        //    //mock the get GetRestaurantAsync method to return a list of restaurant
        //    var restaurants = new List<Restaurant>
        //    {
        //        new Restaurant {Id ="1", Name="Kushi Tsuru", OperatingTime="Mon-Sun 11:30 am - 9 pm",
        //            restaurantTimes = new List<RestaurantTime>{
        //                new RestaurantTime {
        //                    Id="1",
        //                    OpeningDay = "Monday",
        //                    OpeningTime=CommonManagement.GetTimeSpanFromString("11:30:00"),
        //                    ClosingTime=CommonManagement.GetTimeSpanFromString("09:30:00"),
        //                    RestaurantId ="1"},
        //                 new RestaurantTime {
        //                    Id="2",
        //                    OpeningDay = "Tuesday",
        //                    OpeningTime=CommonManagement.GetTimeSpanFromString("11:30:00"),
        //                    ClosingTime=CommonManagement.GetTimeSpanFromString("09:30:00"),
        //                    RestaurantId ="1"}
        //        } },

        //    };

        //    mockRestaurantService.Setup(s => s.GetRestaurantAsync(parameters)).ReturnsAsync(restaurants);
        //    var RestaurantDataService = new IRestaurantDataService(mockRestaurantService.Object);
        //}
    }
}
