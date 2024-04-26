using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Interfaces
{
    public interface IRestaurantTimeService
    {
         Task<List<RestaurantTime>> GetAllRestaurantTimeAsync();
         Task<List<RestaurantTime>> GetRestaurantTimeByRestuarentIdAsync(string id);

    }
}
