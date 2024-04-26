using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Interfaces
{
    public interface IRestaurantRawDataService
    {
        Task<TimeSpan> AddRestaurantBatchAsync(List<Restaurant> restaurant, int batchSize);
    }
}
