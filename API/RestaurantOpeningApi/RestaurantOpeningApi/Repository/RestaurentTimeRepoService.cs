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
        public async Task<List<RestaurantTime>> GetAllRestaurantTimeAsync()
        {
            return await  _context.RestaurantTimes.ToListAsync();
        }

        public async Task<List<RestaurantTime>> GetRestaurantTimeByRestuarentIdAsync(string id)
        {
            return await _context.RestaurantTimes.Where(x=>x.RestaurantId == id).ToListAsync();
        }
    }
}
