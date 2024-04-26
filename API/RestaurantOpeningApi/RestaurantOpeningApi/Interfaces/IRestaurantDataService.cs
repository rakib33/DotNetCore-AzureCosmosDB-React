using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Interfaces
{
    public interface IRestaurantDataService
    {
        Task<TimeSpan> AddRestaurantBatchAsync(List<Restaurant> restaurant, int batchSize);
        Task<List<Restaurant>> GetRestaurantAsync(RestaurantParameters restaurantParameters);
    }
}
