using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Services
{
    public class RestaurantDataService : IRestaurantDataService
    {
      
        private readonly IRestaurantService _restaurantService;
        private readonly IRestaurantTimeService _restaurantTimeService;
        private DateTime Start;
        private TimeSpan TimeSpan;

        public RestaurantDataService( IRestaurantService restaurantService, IRestaurantTimeService restaurantTimeService)
        {   
            _restaurantService = restaurantService;          
            _restaurantTimeService = restaurantTimeService;
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

        public async Task<List<Restaurant>> GetRestaurantAsync()
        {
          return await _restaurantService.GetAllRestaurantAsync();
           
        }
    }
}
