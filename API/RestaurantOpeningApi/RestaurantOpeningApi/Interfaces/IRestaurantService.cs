using Microsoft.Azure.Cosmos;
using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Interfaces
{
    public interface IRestaurantService
    {
        Task<List<Restaurant>> GetAllRestaurantAsync();
        Task AddRestaurantAsync(Restaurant restaurant);      
        Task AddListRestaurantAsync(List<Restaurant> restaurant);      
        void DeleteAsync(string id);
        Task SaveChangesAsync();

    }
}
