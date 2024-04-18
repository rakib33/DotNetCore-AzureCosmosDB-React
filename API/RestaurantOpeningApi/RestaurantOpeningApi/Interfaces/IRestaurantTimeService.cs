using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Interfaces
{
    public interface IRestaurantTimeService
    {
         Task<List<RestaurantTime>> GetAllRestaurantTimeAsync();
         Task AddRestaurantTimeAsync(RestaurantTime restaurantTime);   
        void DeleteAsync(string id);
        Task SaveChangesAsync();
    }
}
