using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Services
{
    public class RestaurantRawDataService : IRestaurantRawDataService
    {
      
        private readonly IRestaurantService _restaurantService;
        private DateTime Start;
        private TimeSpan TimeSpan;

        public RestaurantRawDataService( IRestaurantService restaurantService)
        {   
            _restaurantService = restaurantService;          
        }

        public async Task<TimeSpan> AddRestaurantBatchAsync(List<Restaurant> restaurants, int batchSize)
        {
            Start = DateTime.Now;

            for (int i = 0; i < restaurants.Count; i += batchSize)
            {
                List<Restaurant> batch = restaurants.Skip(i).Take(batchSize).ToList();
                await _restaurantService.AddBulkRestaurantAsync(batch);
                await _restaurantService.SaveChangesAsync();
            }
            
            TimeSpan = DateTime.Now - Start;
            return TimeSpan;
        }
    }
}
