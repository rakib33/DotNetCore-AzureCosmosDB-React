using Microsoft.Azure.Cosmos;
using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Interfaces
{
    public interface IRestaurantService
    {
        Task<List<Restaurant>> GetAllRestaurantAsync(RestaurantParameters restaurantParameters);      
        Task AddBulkRestaurantAsync(List<Restaurant> restaurant);      
        void DeleteAsync(string id);
        Task SaveChangesAsync();

    }
}
