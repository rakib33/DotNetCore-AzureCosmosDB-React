using Microsoft.EntityFrameworkCore;
using RestaurantOpeningApi.DataContext;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Repository
{
    public class RestaurentTimeRepoService : IRestaurantTimeService
    {
        RestaurantContext _context;
        public RestaurentTimeRepoService(RestaurantContext restaurantContext)
        {
            _context = restaurantContext;
        }
        public Task<List<RestaurantTime>> GetAllRestaurantTimeAsync()
        {
            return _context.RestaurantTimes.ToListAsync();
        }

        public Task<List<RestaurantTime>> GetRestaurantTimeByRestuarentIdAsync(string id)
        {
            return _context.RestaurantTimes.Where(x=>x.RestaurantId == id).ToListAsync();
        }
    }
}
