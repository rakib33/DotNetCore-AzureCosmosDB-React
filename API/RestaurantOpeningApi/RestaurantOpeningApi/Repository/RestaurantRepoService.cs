using Microsoft.Azure.Cosmos;
using RestaurantOpeningApi.DataContext;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;


namespace RestaurantOpeningApi.Services
{
    public class RestaurantRepoService : IRestaurantService
    {
        RestaurantContext _context;
        public RestaurantRepoService(RestaurantContext restaurantContext)
        {
            _context = restaurantContext;
        }

        public async Task AddBulkRestaurantAsync(List<Restaurant> restaurant)
        {         
            await  _context.Restaurants.AddRangeAsync(restaurant);              
        }

        public async Task AddRestaurantAsync(Restaurant restaurant)
        {
            try
            {
               await _context.Restaurants.AddAsync(restaurant);                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAsync(string id)
        {
            var restaurant =  _context.Restaurants.Where(t=>t.Id == id).FirstOrDefault();
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
            }
        }

        public Task<List<Restaurant>> GetAllRestaurantAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

   
    }
}
