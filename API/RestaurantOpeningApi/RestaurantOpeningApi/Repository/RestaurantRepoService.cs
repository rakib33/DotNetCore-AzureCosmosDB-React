using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
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
                //disable change tracker
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                //new entities and  don't exist in the database , So apply AsNoTracking to prevent EF Core from tracking them.This improve speed.
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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

        public async Task<List<Restaurant>> GetAllRestaurantAsync()
        {
          List<Restaurant> restaurantData = await   _context.Restaurants.ToListAsync();
         
            //Explicit Loading
            foreach(var restaurant in restaurantData)
            {
               await _context.Entry(restaurant).Collection(p=>p.restaurantTimes).LoadAsync();
            }

            return restaurantData;

        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

   
    }
}
