using Microsoft.EntityFrameworkCore;
using RestaurantOpeningApi.DataContext;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Repository
{
    public class RestaurantTimeRepoService : IRestaurantTimeService
    {
        RestaurantContext _context;
        public RestaurantTimeRepoService(RestaurantContext restaurantContext)
        {
            _context = restaurantContext;
        }
       
        public async Task AddRestaurantTimeAsync(RestaurantTime restaurantTime)
        {
           await _context.RestaurantTimes.AddAsync(restaurantTime);
        }

        public void DeleteAsync(string id)
        {
            var restaurant = _context.RestaurantTimes.Where(t => t.Id == id).FirstOrDefault();
            if (restaurant != null)
            {
                _context.RestaurantTimes.Remove(restaurant);
            }
        }

        public Task<List<Restaurant>> GetAllRestaurantTimeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<RestaurantTime>> GetRestaurantTime()
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        Task<List<RestaurantTime>> IRestaurantTimeService.GetAllRestaurantTimeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
