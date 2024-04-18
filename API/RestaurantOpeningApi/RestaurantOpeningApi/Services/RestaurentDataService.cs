using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;
using System.Net;

namespace RestaurantOpeningApi.Services
{
    public class RestaurentDataService
    {
        private readonly IRestaurantTimeService _restaurantTimeService;
        private readonly IRestaurantService _restaurantService;

        public RestaurentDataService(IRestaurantTimeService restaurantTimeService, IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
            _restaurantTimeService = restaurantTimeService;
        }

        public async Task<bool> UploadRestaurentDataToCosmosDB()
        {
            var RestaurantData = new Restaurant()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test Name 2"
            };
            // Time value as a string
            string timeString1 = "11:30 am";
            string timeString2 = "9 pm";

            // Parse the time string into a TimeOnly structure
            TimeOnly time1 = TimeOnly.Parse(timeString1);
            TimeOnly time2 = TimeOnly.Parse(timeString2);

            var RestaurentTimeData1 = new RestaurantTime()
            {
                Id = Guid.NewGuid().ToString(),
                RestaurantId = RestaurantData.Id,
                Restaurant = RestaurantData,
                OpeningDay = "Mon",
                OpeningTime = time1,
                ClosingTime = time2
            };
            var RestaurentTimeData2 = new RestaurantTime()
            {
                Id = Guid.NewGuid().ToString(),
                RestaurantId = RestaurantData.Id,
                Restaurant = RestaurantData,
                OpeningDay = "Sun",
                OpeningTime = time1,
                ClosingTime = time2
            };

            try
            {
                //add child data
                RestaurantData.restaurantTimes.Add(RestaurentTimeData1);
                RestaurantData.restaurantTimes.Add(RestaurentTimeData2);

                //add parent and child to repository
                await _restaurantService.AddRestaurantAsync(RestaurantData);
                await _restaurantTimeService.AddRestaurantTimeAsync(RestaurentTimeData1);
                await _restaurantTimeService.AddRestaurantTimeAsync(RestaurentTimeData2);

                //Save changes once
                await _restaurantService.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
