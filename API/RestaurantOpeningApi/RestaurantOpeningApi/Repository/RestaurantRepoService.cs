using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using RestaurantOpeningApi.Common;
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

        
        public async Task<List<Restaurant>> GetAllRestaurantAsync(RestaurantParameters p)
        {

           var query =  _context.Restaurants.AsQueryable();
            
            //Apply filtering 
            if (!string.IsNullOrEmpty(p.name))
                query = query.Where(s => s.Name.Contains(p.name));
           
            List<Restaurant> restaurants = await query.ToListAsync();
            //Explicit Loading
            foreach (var restaurant in restaurants)
            {
                if (!string.IsNullOrEmpty(p.day))
                    await _context.Entry(restaurant).Collection(s => s.restaurantTimes.Where(c=>c.OpeningDay.Contains(p.day))).LoadAsync();

                if (p.time != null)
                    await _context.Entry(restaurant).Collection(s => s.restaurantTimes.Where(c => p.time <= c.ClosingTime && p.time >= c.OpeningTime)).LoadAsync();
                else
                    await _context.Entry(restaurant).Collection(p=>p.restaurantTimes).LoadAsync();
            }

            return restaurants.Skip((p.Pagination.Page -1) * p.Pagination.PageSize).Take(p.Pagination.PageSize).ToList();

        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

   
    }
}
